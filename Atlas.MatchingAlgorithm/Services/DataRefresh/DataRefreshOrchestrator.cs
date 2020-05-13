using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Atlas.MatchingAlgorithm.Data.Persistent.Models;
using Atlas.MatchingAlgorithm.Data.Persistent.Repositories;
using Atlas.MatchingAlgorithm.Extensions;
using Atlas.MatchingAlgorithm.Services.AzureManagement;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders.TransientSqlDatabase;
using Atlas.MatchingAlgorithm.ConfigSettings;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders;
using Atlas.MatchingAlgorithm.Services.MatchingDictionary;
using Atlas.Utils.Core.ApplicationInsights;

namespace Atlas.MatchingAlgorithm.Services.DataRefresh
{
    public interface IDataRefreshOrchestrator
    {
        /// <param name="shouldForceRefresh">
        /// If true, the refresh will occur regardless of whether a new hla database version has been published
        /// </param>
        Task RefreshDataIfNecessary(bool shouldForceRefresh = false);
    }

    public class DataRefreshOrchestrator : IDataRefreshOrchestrator
    {
        private readonly ILogger logger;
        private readonly IOptions<DataRefreshSettings> settingsOptions;
        private readonly IHlaMetadataDictionary activeVersionHlaMetadataDictionary;
        private readonly IDataRefreshService dataRefreshService;
        private readonly IDataRefreshHistoryRepository dataRefreshHistoryRepository;
        private readonly IAzureFunctionManager azureFunctionManager;
        private readonly IAzureDatabaseManager azureDatabaseManager;
        private readonly IActiveDatabaseProvider activeDatabaseProvider;
        private readonly IAzureDatabaseNameProvider azureDatabaseNameProvider;
        private readonly IDataRefreshNotificationSender dataRefreshNotificationSender;

        public DataRefreshOrchestrator(
            ILogger logger,
            IOptions<DataRefreshSettings> settingsOptions,
            IHlaMetadataDictionaryFactory hlaMetadataDictionaryFactory,
            IActiveHlaVersionAccessor activeHlaVersionAccessor,
            IActiveDatabaseProvider activeDatabaseProvider,
            IDataRefreshService dataRefreshService,
            IDataRefreshHistoryRepository dataRefreshHistoryRepository,
            IAzureFunctionManager azureFunctionManager,
            IAzureDatabaseManager azureDatabaseManager,
            IAzureDatabaseNameProvider azureDatabaseNameProvider,
            IDataRefreshNotificationSender dataRefreshNotificationSender)
        {
            this.logger = logger;
            this.settingsOptions = settingsOptions;
            this.activeDatabaseProvider = activeDatabaseProvider;
            this.dataRefreshService = dataRefreshService;
            this.dataRefreshHistoryRepository = dataRefreshHistoryRepository;
            this.azureFunctionManager = azureFunctionManager;
            this.azureDatabaseManager = azureDatabaseManager;
            this.azureDatabaseNameProvider = azureDatabaseNameProvider;
            this.dataRefreshNotificationSender = dataRefreshNotificationSender;

            this.activeVersionHlaMetadataDictionary = hlaMetadataDictionaryFactory.BuildDictionary(activeHlaVersionAccessor.GetActiveHlaDatabaseVersion());
        }

        public async Task RefreshDataIfNecessary(bool shouldForceRefresh)
        {
            if (IsRefreshInProgress())
            {
                logger.SendTrace("Data refresh is already in progress. Data Refresh not started.", LogLevel.Info);
                return;
            }

            var newWmdaVersionAvailable = activeVersionHlaMetadataDictionary.IsRefreshNecessary();
            if (!newWmdaVersionAvailable)
            {
                var noNewData = "No new versions of the WMDA HLA nomenclature have been published.";
                if (shouldForceRefresh)
                {
                    logger.SendTrace(noNewData + " But the refresh was run in 'Forced' mode, so a Data Refresh will start anyway.", LogLevel.Info);
                }
                else
                {
                    logger.SendTrace(noNewData + " Data refresh not started.", LogLevel.Info);
                    return;
                }
            }
            else
            {
                logger.SendTrace("A new version of the WMDA HLA nomenclature has been published. Data Refresh will start.", LogLevel.Info);
            }

            await RunDataRefresh();
            logger.SendTrace("Data Refresh ended.", LogLevel.Info);
        }

        private async Task RunDataRefresh()
        {
            await dataRefreshNotificationSender.SendInitialisationNotification();

            var dataRefreshRecord = new DataRefreshRecord
            {
                Database = activeDatabaseProvider.GetDormantDatabase().ToString(),
                RefreshBeginUtc = DateTime.UtcNow,
                WmdaDatabaseVersion = null, //We don't know the version when initially creating the record.
            };

            var recordId = await dataRefreshHistoryRepository.Create(dataRefreshRecord);

            try
            {
                await AzureFunctionsSetUp();
                var newWmdaDataVersion = await dataRefreshService.RefreshData();
                var previouslyActiveDatabase = azureDatabaseNameProvider.GetDatabaseName(activeDatabaseProvider.GetActiveDatabase());
                await MarkDataHistoryRecordAsComplete(recordId, true, newWmdaDataVersion);
                await ScaleDownDatabaseToDormantLevel(previouslyActiveDatabase);
                await dataRefreshNotificationSender.SendSuccessNotification();
                logger.SendTrace("Data Refresh Succeeded.", LogLevel.Info);
            }
            catch (Exception e)
            {
                logger.SendTrace($"Data Refresh Failed: ${e}", LogLevel.Critical);
                await dataRefreshNotificationSender.SendFailureAlert();
                await MarkDataHistoryRecordAsComplete(recordId, false, null);
            }
            finally
            {
                await AzureFunctionsTearDown();
            }
        }

        private async Task AzureFunctionsSetUp()
        {
            var donorFunctionsAppName = settingsOptions.Value.DonorFunctionsAppName;
            var donorImportFunctionName = settingsOptions.Value.DonorImportFunctionName;
            logger.SendTrace($"DATA REFRESH SET UP: Disabling donor import function with name: {donorImportFunctionName}", LogLevel.Info);
            await azureFunctionManager.StopFunction(donorFunctionsAppName, donorImportFunctionName);
        }

        private async Task AzureFunctionsTearDown()
        {
            var donorFunctionsAppName = settingsOptions.Value.DonorFunctionsAppName;
            var donorImportFunctionName = settingsOptions.Value.DonorImportFunctionName;
            logger.SendTrace($"DATA REFRESH TEAR DOWN: Re-enabling donor import function with name: {donorImportFunctionName}", LogLevel.Info);
            await azureFunctionManager.StartFunction(donorFunctionsAppName, donorImportFunctionName);
        }

        private async Task ScaleDownDatabaseToDormantLevel(string databaseName)
        {
            var dormantSize = settingsOptions.Value.DormantDatabaseSize.ToAzureDatabaseSize();
            logger.SendTrace($"DATA REFRESH TEAR DOWN: Scaling down database: {databaseName} to dormant size: {dormantSize}", LogLevel.Info);
            await azureDatabaseManager.UpdateDatabaseSize(databaseName, dormantSize);
        }

        private async Task MarkDataHistoryRecordAsComplete(int recordId, bool wasSuccess, string wmdaDataVersion)
        {
            await dataRefreshHistoryRepository.UpdateExecutionDetails(recordId, wmdaDataVersion, DateTime.UtcNow);
            await dataRefreshHistoryRepository.UpdateSuccessFlag(recordId, wasSuccess);
        }

        private bool IsRefreshInProgress()
        {
            return dataRefreshHistoryRepository.GetInProgressJobs().Any();
        }
    }
}