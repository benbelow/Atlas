﻿using Atlas.Common.GeneticData.Hla.Models;
using Atlas.HlaMetadataDictionary.Repositories;
using System.Collections.Generic;
using System.Linq;
using Atlas.HlaMetadataDictionary.WmdaDataAccess.Models;

namespace Atlas.HlaMetadataDictionary.Services.DataGeneration.AlleleNames
{
    internal interface IAlleleNameHistoriesConsolidator
    {
        IEnumerable<AlleleNameHistory> GetConsolidatedAlleleNameHistories(string hlaNomenclatureVersion);
    }

    internal class AlleleNameHistoriesConsolidator : IAlleleNameHistoriesConsolidator
    {
        private readonly IWmdaDataRepository dataRepository;

        public AlleleNameHistoriesConsolidator(IWmdaDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        public IEnumerable<AlleleNameHistory> GetConsolidatedAlleleNameHistories(string hlaNomenclatureVersion)
        {
            var alleleNameHistories = dataRepository.GetWmdaDataset(hlaNomenclatureVersion).AlleleNameHistories;
            return ConsolidateAlleleNameHistories(alleleNameHistories);
        }

        private static IEnumerable<AlleleNameHistory> ConsolidateAlleleNameHistories(IEnumerable<AlleleNameHistory> alleleNameHistories)
        {
            var alleleNameHistoriesList = alleleNameHistories.ToList();
            var alleleNamesListedInMultipleHistories = GetAlleleNamesListedInMultipleHistories(alleleNameHistoriesList).ToList();
            alleleNamesListedInMultipleHistories.ForEach(allele =>
            {
                var currentHistory = GetFirstHistoryWhereCurrentNameIsNotNull(allele, alleleNameHistoriesList);

                if (currentHistory != null)
                {
                    alleleNameHistoriesList.RemoveAll(history => history.DistinctAlleleNamesContain(allele));
                    alleleNameHistoriesList.Add(currentHistory);
                }
            });

            return alleleNameHistoriesList;
        }

        private static IEnumerable<IWmdaHlaTyping> GetAlleleNamesListedInMultipleHistories(IEnumerable<AlleleNameHistory> alleleNameHistories)
        {
            var alleleNamesListedMoreThanOnce = alleleNameHistories
                .SelectMany(
                    history => history.DistinctAlleleNames,
                    (history, alleleName) => new {Locus = history.TypingLocus, HlaId = history.Name, AlleleName = alleleName})
                .GroupBy(name => new HlaNom(TypingMethod.Molecular, name.Locus, name.AlleleName))
                .Where(grouped => grouped.Count() > 1)
                .Select(grouped => grouped.Key)
                .Distinct();

            return alleleNamesListedMoreThanOnce;
        }

        private static AlleleNameHistory GetFirstHistoryWhereCurrentNameIsNotNull(
            IWmdaHlaTyping alleleTyping,
            IEnumerable<AlleleNameHistory> alleleNameHistories)
        {
            return alleleNameHistories.FirstOrDefault(history =>
                history.DistinctAlleleNamesContain(alleleTyping) && !string.IsNullOrEmpty(history.CurrentAlleleName)
            );
        }
    }
}