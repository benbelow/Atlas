﻿using Atlas.HlaMetadataDictionary.Models.HLATypings;
using Atlas.HlaMetadataDictionary.Models.Lookups.AlleleNameLookup;
using Atlas.HlaMetadataDictionary.Models.Wmda;
using Atlas.HlaMetadataDictionary.Repositories;
using System.Collections.Generic;
using System.Linq;
using Atlas.HlaMetadataDictionary.Services.DataGeneration.AlleleNames;

namespace Atlas.HlaMetadataDictionary.Services.AlleleNames
{
    public interface IAlleleNamesFromHistoriesExtractor
    {
        IEnumerable<AlleleNameLookupResult> GetAlleleNames(string hlaDatabaseVersion);
    }

    public class AlleleNamesFromHistoriesExtractor : AlleleNamesExtractorBase, IAlleleNamesFromHistoriesExtractor
    {
        private readonly IAlleleNameHistoriesConsolidator historiesConsolidator;

        private IEnumerable<AlleleNameHistory> ConsolidatedAlleleNameHistories(string hlaDatabaseVersion)
        {
            return historiesConsolidator.GetConsolidatedAlleleNameHistories(hlaDatabaseVersion).ToList();
        }

        public AlleleNamesFromHistoriesExtractor(
            IAlleleNameHistoriesConsolidator historiesConsolidator,
            IWmdaDataRepository dataRepository)
            : base(dataRepository)
        {
            this.historiesConsolidator = historiesConsolidator;
        }

        public IEnumerable<AlleleNameLookupResult> GetAlleleNames(string hlaDatabaseVersion)
        {
            return ConsolidatedAlleleNameHistories(hlaDatabaseVersion)
                .SelectMany(h => GetAlleleNamesFromSingleHistory(h, hlaDatabaseVersion));
        }

        private IEnumerable<AlleleNameLookupResult> GetAlleleNamesFromSingleHistory(AlleleNameHistory history, string hlaDatabaseVersion)
        {
            var currentAlleleName = GetCurrentAlleleName(history, hlaDatabaseVersion);

            return !string.IsNullOrEmpty(currentAlleleName)
                ? history.ToAlleleNameLookupResults(currentAlleleName)
                : new List<AlleleNameLookupResult>();
        }

        private string GetCurrentAlleleName(AlleleNameHistory history, string hlaDatabaseVersion)
        {
            return history.CurrentAlleleName ?? GetIdenticalToAlleleName(history, hlaDatabaseVersion);
        }

        private string GetIdenticalToAlleleName(AlleleNameHistory history, string hlaDatabaseVersion)
        {
            var mostRecentNameAsTyping = new HlaNom(
                TypingMethod.Molecular, history.TypingLocus, history.MostRecentAlleleName);

            var identicalToAlleleName = AllelesInVersionOfHlaNom(hlaDatabaseVersion)
                .First(allele => allele.TypingEquals(mostRecentNameAsTyping))
                .IdenticalHla;

            return identicalToAlleleName;
        }
    }
}