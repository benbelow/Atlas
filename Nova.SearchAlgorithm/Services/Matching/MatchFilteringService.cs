using System.Linq;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Common.Models.SearchResults;

namespace Nova.SearchAlgorithm.Services.Matching
{
    public interface IMatchFilteringService
    {
        bool FulfilsPerLocusMatchCriteria(MatchResult match, AlleleLevelMatchCriteria criteria, Locus locus);
        bool FulfilsTotalMatchCriteria(MatchResult match, AlleleLevelMatchCriteria criteria);
        bool FulfilsSearchTypeCriteria(MatchResult match, AlleleLevelMatchCriteria criteria);
        bool FulfilsRegistryCriteria(MatchResult match, AlleleLevelMatchCriteria criteria);
    }
    
    public class MatchFilteringService: IMatchFilteringService
    {
        private const int MaximumMatchCountPerLocus = 2;

        public bool FulfilsPerLocusMatchCriteria(MatchResult match, AlleleLevelMatchCriteria criteria, Locus locus)
        {
            var locusMatchDetails = match.MatchDetailsForLocus(locus);
            var locusCriteria = criteria.MatchCriteriaForLocus(locus);
            return locusMatchDetails.MatchCount >= MaximumMatchCountPerLocus - locusCriteria.MismatchCount;
        }

        public bool FulfilsTotalMatchCriteria(MatchResult match, AlleleLevelMatchCriteria criteria)
        {
            return match.TotalMatchCount >= (match.PopulatedLociCount * MaximumMatchCountPerLocus) - criteria.DonorMismatchCount;
        }

        public bool FulfilsSearchTypeCriteria(MatchResult match, AlleleLevelMatchCriteria criteria)
        {
            return match.Donor.DonorType == criteria.SearchType;
        }

        public bool FulfilsRegistryCriteria(MatchResult match, AlleleLevelMatchCriteria criteria)
        {
            return criteria.RegistriesToSearch.Contains(match.Donor.RegistryCode);
        }
    }
}