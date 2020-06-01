using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Atlas.Common.Utils;
using Atlas.HlaMetadataDictionary.ExternalInterface;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders;
using Atlas.MultipleAlleleCodeDictionary;
using Microsoft.Azure.WebJobs;

namespace Atlas.MatchingAlgorithm.Functions.DonorManagement.Functions
{
    public class CachingFunctions
    {
        private readonly IAntigenCachingService antigenCachingService;
        private readonly IHlaMetadataCacheControl hlaMetadataCacheControl;

        public CachingFunctions(
            IAntigenCachingService antigenCachingService,
            IHlaMetadataDictionaryFactory hlaMetadataDictionaryFactory,
            IActiveHlaVersionAccessor hlaVersionAccessor
        )
        {
            this.antigenCachingService = antigenCachingService;
            hlaMetadataCacheControl = hlaMetadataDictionaryFactory.BuildCacheControl(hlaVersionAccessor.GetActiveHlaNomenclatureVersion());
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