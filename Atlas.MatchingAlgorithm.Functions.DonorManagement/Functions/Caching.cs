using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Atlas.MatchingAlgorithm.Services.MatchingDictionary;
using Atlas.MultipleAlleleCodeDictionary;
using Atlas.Utils.CodeAnalysis;

namespace Atlas.MatchingAlgorithm.Functions.DonorManagement.Functions
{
    public class Caching
    {
        private readonly IAntigenCachingService antigenCachingService;
        private readonly IHlaMetadataCacheControl hlaMetadataCacheControl;

        public Caching(
            IAntigenCachingService antigenCachingService,
            IHlaMetadataCacheControl hlaMetadataCacheControl
        )
        {
            this.antigenCachingService = antigenCachingService;
            this.hlaMetadataCacheControl = hlaMetadataCacheControl;
        }

        [SuppressMessage(null, SuppressMessage.UnusedParameter, Justification = SuppressMessage.UsedByAzureTrigger)]
        [FunctionName(nameof(UpdateHlaCache))]
        public async Task UpdateHlaCache(
            [TimerTrigger("00 00 03 * * *", RunOnStartup = true)]
            TimerInfo timerInfo)
        {
            await antigenCachingService.GenerateAntigenCache();
        }

        [SuppressMessage(null, SuppressMessage.UnusedParameter, Justification = SuppressMessage.UsedByAzureTrigger)]
        [FunctionName(nameof(UpdateHlaMetadataDictionaryCache))]
        public async Task UpdateHlaMetadataDictionaryCache(
            [TimerTrigger("00 00 03 * * *", RunOnStartup = true)]
            TimerInfo timerInfo)
        {
            await hlaMetadataCacheControl.PreWarmAlleleNameCache();
        }
    }
}