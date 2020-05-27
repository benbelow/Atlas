﻿using System;
using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.Hla.Models;
using Atlas.HlaMetadataDictionary.Models.Lookups.ScoringLookup;
using Atlas.HlaMetadataDictionary.Test.IntegrationTests.TestHelpers.ScoringInfoBuilders;

namespace Atlas.MatchingAlgorithm.Test.TestHelpers.Builders
{
    internal class HlaScoringLookupResultBuilder
    {
        private HlaScoringLookupResult result;

        public HlaScoringLookupResultBuilder()
        {
            result = new HlaScoringLookupResult(
                Locus.A,
                // Scoring information is cached per-lookup name - so these should be unique by default to avoid cache key collision
                Guid.NewGuid().ToString(),
                new SingleAlleleScoringInfoBuilder().Build(),
                TypingMethod.Molecular
            );
        }

        public HlaScoringLookupResultBuilder AtLocus(Locus locus)
        {
            result = new HlaScoringLookupResult(locus, result.LookupName, result.HlaScoringInfo, result.TypingMethod);
            return this;
        }

        public HlaScoringLookupResultBuilder WithLookupName(string lookupName)
        {
            result = new HlaScoringLookupResult(result.Locus, lookupName, result.HlaScoringInfo, result.TypingMethod);
            return this;
        }

        public HlaScoringLookupResultBuilder WithHlaScoringInfo(IHlaScoringInfo scoringInfo)
        {
            result = new HlaScoringLookupResult(result.Locus, result.LookupName, scoringInfo, result.TypingMethod);
            return this;
        }
        
        public HlaScoringLookupResultBuilder WithTypingMethod(TypingMethod typingMethod)
        {
            result = new HlaScoringLookupResult(result.Locus, result.LookupName, result.HlaScoringInfo, typingMethod);
            return this;
        }

        public HlaScoringLookupResult Build()
        {
            return result;
        }
    }
}