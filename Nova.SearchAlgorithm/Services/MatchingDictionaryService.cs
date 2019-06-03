using System.Collections.Generic;
using System.Threading.Tasks;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups.MatchingLookup;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups.ScoringLookup;
using Nova.SearchAlgorithm.MatchingDictionary.Services;

namespace Nova.SearchAlgorithm.Services
{
    public interface IMatchingDictionaryService
    {
        Task RecreateMatchingDictionary();
        Task<IEnumerable<string>> GetCurrentAlleleNames(Locus locus, string alleleLookupName);
        Task<IHlaMatchingLookupResult> GetHlaMatchingLookupResult(Locus locus, string hlaName);
        Task<IHlaScoringLookupResult> GetHlaScoringLookupResult(Locus locus, string hlaName);
        Task<string> GetDpb1TceGroup(string dpb1HlaName);
        HlaLookupResultCollections GetAllHlaLookupResults();
    }
    
    public class MatchingDictionaryService: IMatchingDictionaryService
    {
        private readonly IRecreateHlaLookupResultsService manageMatchingService;
        private readonly IAlleleNamesLookupService alleleNamesLookupService;
        private readonly IHlaMatchingLookupService hlaMatchingLookupService;
        private readonly IHlaScoringLookupService hlaScoringLookupService;
        private readonly IHlaLookupResultsService hlaLookupResultsService;
        private readonly IDpb1TceGroupLookupService dpb1TceGroupLookupService;
        private readonly string hlaDatabaseVersion;

        public MatchingDictionaryService(
            IRecreateHlaLookupResultsService manageMatchingService,
            IAlleleNamesLookupService alleleNamesLookupService,
            IHlaMatchingLookupService hlaMatchingLookupService,
            IHlaScoringLookupService hlaScoringLookupService,
            IHlaLookupResultsService hlaLookupResultsService,
            IDpb1TceGroupLookupService dpb1TceGroupLookupService,
            string hlaDatabaseVersion)
        {
            this.manageMatchingService = manageMatchingService;
            this.alleleNamesLookupService = alleleNamesLookupService;
            this.hlaMatchingLookupService = hlaMatchingLookupService;
            this.hlaScoringLookupService = hlaScoringLookupService;
            this.hlaLookupResultsService = hlaLookupResultsService;
            this.dpb1TceGroupLookupService = dpb1TceGroupLookupService;
            this.hlaDatabaseVersion = hlaDatabaseVersion;
        }

        public async Task RecreateMatchingDictionary()
        {
            await manageMatchingService.RecreateAllHlaLookupResults(hlaDatabaseVersion);
        }

        public async Task<IEnumerable<string>> GetCurrentAlleleNames(Locus locus, string alleleLookupName)
        {
            return await alleleNamesLookupService.GetCurrentAlleleNames(locus, alleleLookupName, hlaDatabaseVersion);
        }

        public async Task<IHlaMatchingLookupResult> GetHlaMatchingLookupResult(Locus locus, string hlaName)
        {
            return await hlaMatchingLookupService.GetHlaLookupResult(locus, hlaName, hlaDatabaseVersion);
        }

        public async Task<IHlaScoringLookupResult> GetHlaScoringLookupResult(Locus locus, string hlaName)
        {
            return await hlaScoringLookupService.GetHlaLookupResult(locus, hlaName, hlaDatabaseVersion);
        }

        public async Task<string> GetDpb1TceGroup(string dpb1HlaName)
        {
            return await dpb1TceGroupLookupService.GetDpb1TceGroup(dpb1HlaName, hlaDatabaseVersion);
        }

        public HlaLookupResultCollections GetAllHlaLookupResults()
        {
            return hlaLookupResultsService.GetAllHlaLookupResults(hlaDatabaseVersion);
        }
    }
}