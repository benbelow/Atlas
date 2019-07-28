using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nova.SearchAlgorithm.Common.Repositories.DonorUpdates;
using Nova.SearchAlgorithm.Data.Persistent.Models;
using Nova.SearchAlgorithm.Extensions;
using Nova.SearchAlgorithm.MatchingDictionary.Services;
using Nova.SearchAlgorithm.Models.AzureManagement;
using Nova.SearchAlgorithm.Services.AzureManagement;
using Nova.SearchAlgorithm.Services.ConfigurationProviders;
using Nova.SearchAlgorithm.Settings;
using Nova.Utils.ApplicationInsights;

namespace Nova.SearchAlgorithm.Services.DataRefresh
{
    public interface IDataRefreshService
    {
        /// <summary>
        /// Performs all pre-processing required for running of the search algorithm:
        /// - Scales up target database 
        /// - Recreates Matching Dictionary
        /// - Imports all donors
        /// - Processes HLA for imported donors
        /// - Scales down target database
        /// </summary>
        Task RefreshData(string wmdaDatabaseVersion);
    }

    public class DataRefreshService : IDataRefreshService
    {
        private readonly IOptions<DataRefreshSettings> settingsOptions;
        private readonly IActiveDatabaseProvider activeDatabaseProvider;
        private readonly IAzureDatabaseNameProvider azureDatabaseNameProvider;
        private readonly IAzureDatabaseManager azureDatabaseManager;

        private readonly IDonorImportRepository donorImportRepository;

        private readonly IRecreateHlaLookupResultsService recreateMatchingDictionaryService;
        private readonly IDonorImporter donorImporter;
        private readonly IHlaProcessor hlaProcessor;
        private readonly ILogger logger;

        public DataRefreshService(
            IOptions<DataRefreshSettings> dataRefreshSettingsOptions,
            IActiveDatabaseProvider activeDatabaseProvider,
            IAzureDatabaseNameProvider azureDatabaseNameProvider,
            IAzureDatabaseManager azureDatabaseManager,
            IDonorImportRepository donorImportRepository,
            IRecreateHlaLookupResultsService recreateMatchingDictionaryService,
            IDonorImporter donorImporter,
            IHlaProcessor hlaProcessor,
            ILogger logger)
        {
            this.activeDatabaseProvider = activeDatabaseProvider;
            this.azureDatabaseNameProvider = azureDatabaseNameProvider;
            this.azureDatabaseManager = azureDatabaseManager;
            this.donorImportRepository = donorImportRepository;
            this.recreateMatchingDictionaryService = recreateMatchingDictionaryService;
            this.donorImporter = donorImporter;
            this.hlaProcessor = hlaProcessor;
            this.logger = logger;
            settingsOptions = dataRefreshSettingsOptions;
        }

        public async Task RefreshData(string wmdaDatabaseVersion)
        {
            try
            {
                await RecreateMatchingDictionary(wmdaDatabaseVersion);
                await RemoveExistingDonorData();
                await ScaleDatabase(settingsOptions.Value.RefreshDatabaseSize.ToAzureDatabaseSize());
                await ImportDonors();
                await ProcessDonorHla(wmdaDatabaseVersion);
                await ScaleDatabase(settingsOptions.Value.ActiveDatabaseSize.ToAzureDatabaseSize());
            }
            catch (Exception)
            {
                logger.SendTrace($"DATA REFRESH: Refresh failed. Scaling down database to dormant size: {wmdaDatabaseVersion}", LogLevel.Info);
                await ScaleDatabase(settingsOptions.Value.DormantDatabaseSize.ToAzureDatabaseSize());
                throw;
            }
        }

        private async Task RecreateMatchingDictionary(string wmdaDatabaseVersion)
        {
            logger.SendTrace($"DATA REFRESH: Recreating matching dictionary for hla database version: {wmdaDatabaseVersion}", LogLevel.Info);
            await recreateMatchingDictionaryService.RecreateAllHlaLookupResults(wmdaDatabaseVersion);
        }

        private async Task RemoveExistingDonorData()
        {
            logger.SendTrace("DATA REFRESH: Removing existing donor data", LogLevel.Info);
            await donorImportRepository.RemoveAllDonorInformation();
        }

        private async Task ImportDonors()
        {
            logger.SendTrace("DATA REFRESH: Importing Donors", LogLevel.Info);
            await donorImporter.ImportDonors();
        }

        private async Task ProcessDonorHla(string wmdaDatabaseVersion)
        {
            logger.SendTrace($"DATA REFRESH: Processing Donor hla using hla database version: {wmdaDatabaseVersion}", LogLevel.Info);
            await hlaProcessor.UpdateDonorHla(wmdaDatabaseVersion);
        }

        private async Task ScaleDatabase(AzureDatabaseSize targetSize)
        {
            var databaseName = azureDatabaseNameProvider.GetDatabaseName(activeDatabaseProvider.GetDormantDatabase());
            logger.SendTrace($"DATA REFRESH: Scaling database: {databaseName} to size {targetSize}", LogLevel.Info);
            await azureDatabaseManager.UpdateDatabaseSize(databaseName, targetSize);
        }
    }
}