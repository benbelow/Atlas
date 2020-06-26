﻿using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.Common.Helpers;

namespace Atlas.MatchPrediction.Services.GenotypeLikelihood
{
    public interface IGenotypeAlleleTruncater
    {
        /// <summary>
        /// This will only be used when we implement 2-field HF sets.
        /// </summary>
        public PhenotypeInfo<string> TruncateGenotypeAlleles(PhenotypeInfo<string> genotype);
    }

    public class GenotypeAlleleTruncater : IGenotypeAlleleTruncater
    {
        public PhenotypeInfo<string> TruncateGenotypeAlleles(PhenotypeInfo<string> genotype)
        {
            return genotype.Map((locus, position, allele) =>
                allele == null ? null : AlleleSplitter.FirstTwoFieldsAsString(allele));
        }
    }
}