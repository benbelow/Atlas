using System.Threading.Tasks;
using Atlas.Client.Models.Search.Results.Matching;
using Atlas.Common.ApplicationInsights;
using Atlas.Common.ApplicationInsights.Timing;
using Atlas.Common.AzureStorage.Blob;
using Atlas.Functions.Settings;
using Microsoft.Extensions.Options;

namespace Atlas.Functions.Services.BlobStorageClients
{
    public interface IMatchingResultsDownloader
    {
        public Task<MatchingAlgorithmResultSet> Download(string blobName, bool isRepeatSearch);
    }

    internal class MatchingResultsDownloader : IMatchingResultsDownloader
    {
        private readonly AzureStorageSettings messagingServiceBusSettings;
        private readonly IBlobDownloader blobDownloader;
        private readonly ILogger logger;

        public MatchingResultsDownloader(IOptions<AzureStorageSettings> messagingServiceBusSettings, IBlobDownloader blobDownloader, ILogger logger)
        {
            this.messagingServiceBusSettings = messagingServiceBusSettings.Value;
            this.blobDownloader = blobDownloader;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<MatchingAlgorithmResultSet> Download(string blobName, bool isRepeatSearch)
        {
            using (logger.RunTimed($"Downloading matching results: {blobName}"))
            {
                var matchingResultsBlobContainer = isRepeatSearch
                    ? messagingServiceBusSettings.RepeatSearchMatchingResultsBlobContainer
                    : messagingServiceBusSettings.MatchingResultsBlobContainer;
                return await blobDownloader.Download<MatchingAlgorithmResultSet>(matchingResultsBlobContainer, blobName);
            }
        }
    }
}