﻿using System.Collections.Generic;
using System.Linq;
using Atlas.HlaMetadataDictionary.Models.Lookups.AlleleNameLookup;
using Atlas.HlaMetadataDictionary.Services.DataGeneration.AlleleNames;

namespace Atlas.HlaMetadataDictionary.Services.DataGeneration
{
    /// <summary>
    /// Generates a complete collection of Allele Names.
    /// </summary>
    internal interface IAlleleNamesService
    {
        IEnumerable<IAlleleNameLookupResult> GetAlleleNamesAndTheirVariants(string hlaNomenclatureVersion);
    }

    /// <inheritdoc />
    /// <summary>
    /// Extracts allele names from a WMDA data repository.
    /// </summary>
    internal class AlleleNamesService : IAlleleNamesService
    {
        private readonly IAlleleNamesFromHistoriesExtractor alleleNamesFromHistoriesExtractor;
        private readonly IAlleleNameVariantsExtractor alleleNameVariantsExtractor;
        private readonly IReservedAlleleNamesExtractor reservedAlleleNamesExtractor;
        
        public AlleleNamesService(
            IAlleleNamesFromHistoriesExtractor alleleNamesFromHistoriesExtractor,
            IAlleleNameVariantsExtractor alleleNameVariantsExtractor,
            IReservedAlleleNamesExtractor reservedAlleleNamesExtractor)
        {
            this.alleleNamesFromHistoriesExtractor = alleleNamesFromHistoriesExtractor;
            this.alleleNameVariantsExtractor = alleleNameVariantsExtractor;
            this.reservedAlleleNamesExtractor = reservedAlleleNamesExtractor;
        }

        public IEnumerable<IAlleleNameLookupResult> GetAlleleNamesAndTheirVariants(string hlaNomenclatureVersion)
        {
            var alleleNamesFromHistories = GetAlleleNamesFromHistories(hlaNomenclatureVersion).ToList();
            var nameVariants = GetAlleleNameVariants(alleleNamesFromHistories, hlaNomenclatureVersion).ToList();
            var reservedNames = GetReservedAlleleNames(hlaNomenclatureVersion).ToList();

            var mergedCollectionOfAlleleNames = alleleNamesFromHistories
                .Concat(nameVariants)
                .Concat(reservedNames)
                .ToList();

            return mergedCollectionOfAlleleNames;
        }

        private IEnumerable<IAlleleNameLookupResult> GetAlleleNamesFromHistories(string hlaNomenclatureVersion)
        {
            return alleleNamesFromHistoriesExtractor.GetAlleleNames(hlaNomenclatureVersion);
        }

        private IEnumerable<IAlleleNameLookupResult> GetAlleleNameVariants(IEnumerable<IAlleleNameLookupResult> originalAlleleNames, string hlaNomenclatureVersion)
        {
            return alleleNameVariantsExtractor.GetAlleleNames(originalAlleleNames, hlaNomenclatureVersion);
        }

        private IEnumerable<IAlleleNameLookupResult> GetReservedAlleleNames(string hlaNomenclatureVersion)
        {
            return reservedAlleleNamesExtractor.GetAlleleNames(hlaNomenclatureVersion);
        }
    }
}

