using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Atlas.HlaMetadataDictionary.Repositories;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders;
using Atlas.MultipleAlleleCodeDictionary;
using Atlas.Utils.CodeAnalysis;

//QQ The entry point should remain here, but the details should gets pushed into the HlaMetadataDictionary project?
namespace Atlas.MatchingAlgorithm.Functions.Functions
{
    public class Caching
    {
        private readonly IAntigenCachingService antigenCachingService;
        private readonly IHlaMatchingLookupRepository matchingLookupRepository;
        private readonly IAlleleNamesLookupRepository alleleNamesLookupRepository;
        private readonly IHlaScoringLookupRepository scoringLookupRepository;
        private readonly IDpb1TceGroupsLookupRepository dpb1TceGroupsLookupRepository;
        private readonly IActiveHlaVersionAccessor hlaVersionProvider;

        public Caching(
            IAntigenCachingService antigenCachingService,
            IHlaMatchingLookupRepository matchingLookupRepository,
            IAlleleNamesLookupRepository alleleNamesLookupRepository,
            IHlaScoringLookupRepository scoringLookupRepository,
            IDpb1TceGroupsLookupRepository dpb1TceGroupsLookupRepository,
            IActiveHlaVersionAccessor wmdaHlaVersionProvider
        )
        {
            this.antigenCachingService = antigenCachingService;
            this.matchingLookupRepository = matchingLookupRepository;
            this.alleleNamesLookupRepository = alleleNamesLookupRepository;
            this.scoringLookupRepository = scoringLookupRepository;
            this.dpb1TceGroupsLookupRepository = dpb1TceGroupsLookupRepository;
            this.hlaVersionProvider = wmdaHlaVersionProvider;
        }

        [SuppressMessage(null, SuppressMessage.UnusedParameter, Justification = SuppressMessage.UsedByAzureTrigger)]
        [FunctionName("UpdateHlaCache")]
        public async Task UpdateHlaCache(
            [TimerTrigger("00 00 02 * * *", RunOnStartup = true)]
            TimerInfo timerInfo)
        {
            await antigenCachingService.GenerateAntigenCache();
        }

        [SuppressMessage(null, SuppressMessage.UnusedParameter, Justification = SuppressMessage.UsedByAzureTrigger)]
        [FunctionName("UpdateMatchingDictionaryCache")]
        public async Task UpdateMatchingDictionaryCache(
            [TimerTrigger("00 00 02 * * *", RunOnStartup = true)]
            TimerInfo timerInfo)
        {
            var hlaDatabaseVersion = hlaVersionProvider.GetActiveHlaDatabaseVersion();
            await matchingLookupRepository.LoadDataIntoMemory(hlaDatabaseVersion);
            await alleleNamesLookupRepository.LoadDataIntoMemory(hlaDatabaseVersion);
            await scoringLookupRepository.LoadDataIntoMemory(hlaDatabaseVersion);
            await dpb1TceGroupsLookupRepository.LoadDataIntoMemory(hlaDatabaseVersion);
        }
    }
}