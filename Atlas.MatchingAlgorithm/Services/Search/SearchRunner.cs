using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Atlas.Common.ApplicationInsights;
using Atlas.MatchingAlgorithm.ApplicationInsights.SearchRequests;
using Atlas.MatchingAlgorithm.Client.Models.SearchResults;
using Atlas.MatchingAlgorithm.Clients.AzureStorage;
using Atlas.MatchingAlgorithm.Clients.ServiceBus;
using Atlas.MatchingAlgorithm.Common.Models;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders;

namespace Atlas.MatchingAlgorithm.Services.Search
{
    public interface ISearchRunner
    {
        Task<MatchingAlgorithmResultSet> RunSearch(IdentifiedSearchRequest identifiedSearchRequest);
    }

    public class SearchRunner : ISearchRunner
    {
        private readonly ISearchServiceBusClient searchServiceBusClient;
        private readonly ISearchService searchService;
        private readonly IResultsBlobStorageClient resultsBlobStorageClient;
        private readonly ILogger logger;
        private readonly ISearchRequestContext searchRequestContext;
        private readonly IActiveHlaNomenclatureVersionAccessor hlaNomenclatureVersionAccessor;

        public SearchRunner(
            ISearchServiceBusClient searchServiceBusClient,
            ISearchService searchService,
            IResultsBlobStorageClient resultsBlobStorageClient,
            ILogger logger,
            ISearchRequestContext searchRequestContext,
            IActiveHlaNomenclatureVersionAccessor hlaNomenclatureVersionAccessor)
        {
            this.searchServiceBusClient = searchServiceBusClient;
            this.searchService = searchService;
            this.resultsBlobStorageClient = resultsBlobStorageClient;
            this.logger = logger;
            this.searchRequestContext = searchRequestContext;
            this.hlaNomenclatureVersionAccessor = hlaNomenclatureVersionAccessor;
        }

        public async Task<MatchingAlgorithmResultSet> RunSearch(IdentifiedSearchRequest identifiedSearchRequest)
        {
            var searchRequestId = identifiedSearchRequest.Id;
            searchRequestContext.SearchRequestId = searchRequestId;
            var searchAlgorithmServiceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            var hlaNomenclatureVersion = hlaNomenclatureVersionAccessor.GetActiveHlaNomenclatureVersion();

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var results = (await searchService.Search(identifiedSearchRequest.MatchingRequest)).ToList();
                stopwatch.Stop();

                var blobContainerName = resultsBlobStorageClient.GetResultsContainerName();

                var searchResultSet = new MatchingAlgorithmResultSet
                {
                    SearchRequestId = searchRequestId,
                    MatchingAlgorithmResults = results,
                    ResultCount = results.Count,
                    HlaNomenclatureVersion = hlaNomenclatureVersion,
                    BlobStorageContainerName = blobContainerName,
                };

                await resultsBlobStorageClient.UploadResults(searchResultSet);

                var notification = new SearchResultsNotification
                {
                    SearchRequestId = searchRequestId,
                    SearchAlgorithmServiceVersion = searchAlgorithmServiceVersion,
                    HlaNomenclatureVersion = hlaNomenclatureVersion,
                    WasSuccessful = true,
                    NumberOfResults = results.Count,
                    BlobStorageContainerName = blobContainerName,
                    SearchTimeInMilliseconds = stopwatch.ElapsedMilliseconds
                };
                await searchServiceBusClient.PublishToResultsNotificationTopic(notification);
                return searchResultSet;
            }
            catch (Exception e)
            {
                logger.SendTrace($"Failed to run search with id {searchRequestId}. Exception: {e}", LogLevel.Error);
                var notification = new SearchResultsNotification
                {
                    WasSuccessful = false,
                    SearchRequestId = searchRequestId,
                    SearchAlgorithmServiceVersion = searchAlgorithmServiceVersion,
                    HlaNomenclatureVersion = hlaNomenclatureVersion
                };
                await searchServiceBusClient.PublishToResultsNotificationTopic(notification);
                throw;
            }
        }
    }
}