﻿using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingDictionary;
using Nova.SearchAlgorithm.MatchingDictionary.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nova.SearchAlgorithm.MatchingDictionary.Services.MatchingDictionary.Lookups
{
    internal abstract class MultipleAllelesLookup : MatchingDictionaryLookup
    {
        protected MultipleAllelesLookup(IMatchingDictionaryRepository dictionaryRepository) : base(dictionaryRepository)
        {
        }

        public override async Task<MatchingDictionaryEntry> PerformLookupAsync(MatchLocus matchLocus, string lookupName)
        {
            var alleles = await GetAlleles(matchLocus, lookupName);
            var tasks = alleles.Select(allele => GetMatchingDictionaryEntry(matchLocus, allele, TypingMethod.Molecular));
            var entries = await Task.WhenAll(tasks);

            return new MatchingDictionaryEntry(matchLocus, lookupName, MolecularSubtype.MultipleAlleles, entries);
        }

        protected abstract Task<IEnumerable<string>> GetAlleles(MatchLocus matchLocus, string lookupName);
    }
}
