﻿using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingTypings;

namespace Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingDictionary
{
    /// <summary>
    /// Lookup result with data required to match HLA pairings.
    /// </summary>
    public interface IHlaMatchingLookupResult : IHlaLookupResult, IMatchingPGroups
    {
    }
}
