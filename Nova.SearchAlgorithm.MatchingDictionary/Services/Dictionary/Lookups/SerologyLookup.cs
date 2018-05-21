﻿using System.Threading.Tasks;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Dictionary;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypes;
using Nova.SearchAlgorithm.MatchingDictionary.Repositories;

namespace Nova.SearchAlgorithm.MatchingDictionary.Services.Dictionary.Lookups
{
    /// <summary>
    /// This class is responsible for
    /// looking up a serology typing in the matching dictionary.
    /// </summary>
    internal class SerologyLookup : MatchingDictionaryLookup
    {
        public SerologyLookup(IMatchedHlaRepository dictionaryRepository) : base(dictionaryRepository)
        {
        }

        public override Task<MatchingDictionaryEntry> PerformLookupAsync(MatchLocus matchLocus, string lookupName)
        {
            return GetDictionaryEntry(matchLocus, lookupName, TypingMethod.Serology);
        }
    }
}
