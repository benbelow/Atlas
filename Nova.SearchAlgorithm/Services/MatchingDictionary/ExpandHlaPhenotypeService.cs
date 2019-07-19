﻿using System;
using System.Threading.Tasks;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Extensions.MatchingDictionaryConversionExtensions;
using Nova.SearchAlgorithm.MatchingDictionary.Services;
using Nova.SearchAlgorithm.Services.ConfigurationProviders;

namespace Nova.SearchAlgorithm.Services.MatchingDictionary
{
    public interface IExpandHlaPhenotypeService
    {
        Task<PhenotypeInfo<ExpandedHla>> GetPhenotypeOfExpandedHla(PhenotypeInfo<string> hlaPhenotype, string hlaDatabaseVersion = null);
    }

    /// <inheritdoc />
    public class ExpandHlaPhenotypeService : IExpandHlaPhenotypeService
    {
        private readonly ILocusHlaMatchingLookupService locusHlaLookupService;
        private readonly IWmdaHlaVersionProvider wmdaHlaVersionProvider;

        public ExpandHlaPhenotypeService(ILocusHlaMatchingLookupService locusHlaLookupService, IWmdaHlaVersionProvider wmdaHlaVersionProvider)
        {
            this.locusHlaLookupService = locusHlaLookupService;
            this.wmdaHlaVersionProvider = wmdaHlaVersionProvider;
        }

        public async Task<PhenotypeInfo<ExpandedHla>> GetPhenotypeOfExpandedHla(PhenotypeInfo<string> hlaPhenotype, string hlaDatabaseVersion)
        {
            if (hlaDatabaseVersion == null)
            {
                hlaDatabaseVersion = wmdaHlaVersionProvider.GetActiveHlaDatabaseVersion();
            }

            return await hlaPhenotype.WhenAllLoci((l, h1, h2) => GetExpandedHla(l, h1, h2, hlaDatabaseVersion));
        }

        private async Task<Tuple<ExpandedHla, ExpandedHla>> GetExpandedHla(Locus locus, string hla1, string hla2, string hlaDatabaseVersion)
        {
            if (hla1 == null || hla2 == null)
            {
                return new Tuple<ExpandedHla, ExpandedHla>(null, null);
            }

            var (item1, item2) =
                await locusHlaLookupService.GetHlaMatchingLookupResults(locus, new Tuple<string, string>(hla1, hla2), hlaDatabaseVersion);

            return new Tuple<ExpandedHla, ExpandedHla>(
                item1.ToExpandedHla(hla1),
                item2.ToExpandedHla(hla2));
        }
    }
}