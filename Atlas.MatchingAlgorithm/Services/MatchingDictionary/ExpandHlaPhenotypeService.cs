﻿using System;
using System.Threading.Tasks;
using Atlas.MatchingAlgorithm.Common.Models;
using Atlas.MatchingAlgorithm.Extensions.MatchingDictionaryConversionExtensions;
using Atlas.HlaMetadataDictionary.Services;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders;

//QQ this whole class is moving into HlaMdDict
namespace Atlas.MatchingAlgorithm.Services.MatchingDictionary
{
    public interface IExpandHlaPhenotypeService
    {
        //QQ Remove optional input here.
        Task<PhenotypeInfo<ExpandedHla>> GetPhenotypeOfExpandedHla(PhenotypeInfo<string> hlaPhenotype, string hlaDatabaseVersion = null);
    }

    /// <inheritdoc />
    public class ExpandHlaPhenotypeService : IExpandHlaPhenotypeService
    {
        private readonly ILocusHlaMatchingLookupService locusHlaLookupService;
        private readonly IActiveHlaVersionAccessor activeHlaVersionProvider;

        public ExpandHlaPhenotypeService(
            ILocusHlaMatchingLookupService locusHlaLookupService,
            IActiveHlaVersionAccessor activeHlaVersionProvider)
        {
            this.locusHlaLookupService = locusHlaLookupService;
            this.activeHlaVersionProvider = activeHlaVersionProvider;
        }

        public async Task<PhenotypeInfo<ExpandedHla>> GetPhenotypeOfExpandedHla(PhenotypeInfo<string> hlaPhenotype, string hlaDatabaseVersion)
        {
            if (hlaDatabaseVersion == null)
            {
                hlaDatabaseVersion = activeHlaVersionProvider.GetActiveHlaDatabaseVersion();
            }

            return await hlaPhenotype.WhenAllLoci((l, h1, h2) => GetExpandedHla(l, h1, h2, hlaDatabaseVersion));
        }

        private async Task<Tuple<ExpandedHla, ExpandedHla>> GetExpandedHla(Locus locus, string hla1, string hla2, string hlaDatabaseVersion)
        {
            if (string.IsNullOrEmpty(hla1) || string.IsNullOrEmpty(hla2))
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