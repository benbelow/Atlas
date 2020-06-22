﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.Common.Matching.Services;
using Atlas.HlaMetadataDictionary.ExternalInterface.Models;
using Atlas.MatchPrediction.Config;

namespace Atlas.MatchPrediction.Services.MatchCalculation
{
    public interface IMatchCalculationService
    {
        public Task<LociInfo<int>> MatchAtPGroupLevel(
            PhenotypeInfo<string> patientGenotype,
            PhenotypeInfo<string> donorGenotype,
            string hlaNomenclatureVersion);
    }

    public class MatchCalculationService : IMatchCalculationService
    {
        private readonly ILocusHlaConverter locusHlaConverter;
        private readonly ILocusMatchCalculator locusMatchCalculator;

        public MatchCalculationService(
            ILocusHlaConverter locusHlaConverter, 
            ILocusMatchCalculator locusMatchCalculator)
        {
            this.locusHlaConverter = locusHlaConverter;
            this.locusMatchCalculator = locusMatchCalculator;
        }

        public async Task<LociInfo<int>> MatchAtPGroupLevel(
            PhenotypeInfo<string> patientGenotype,
            PhenotypeInfo<string> donorGenotype,
            string hlaNomenclatureVersion)
        {
            const TargetHlaCategory matchingResolution = TargetHlaCategory.PGroup;

            var patientGenotypeAsPGroups =
                await locusHlaConverter.ConvertHla(patientGenotype, matchingResolution, hlaNomenclatureVersion);
            var donorGenotypeAsPGroups =
                await locusHlaConverter.ConvertHla(donorGenotype, matchingResolution, hlaNomenclatureVersion);

            var allowedLoci = LocusSettings.MatchPredictionLoci.ToList();

            var matchCounts = new LociInfo<int>().Map((locus, matchCount) =>
                allowedLoci.Contains(locus)
                    ? locusMatchCalculator.MatchCount(
                        patientGenotypeAsPGroups.GetLocus(locus).Map(x => x as IEnumerable<string>),
                        donorGenotypeAsPGroups.GetLocus(locus).Map(x => x as IEnumerable<string>))
                    : 0);

            return matchCounts;
        }
    }
}