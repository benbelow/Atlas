﻿using System.Collections.Generic;
using Atlas.HlaMetadataDictionary.ExternalInterface.Models.Metadata;
using Atlas.HlaMetadataDictionary.ExternalInterface.Models.Metadata.ScoringMetadata;

namespace Atlas.HlaMetadataDictionary.Test.TestHelpers.Builders.ScoringInfoBuilders
{
    public class MultipleAlleleScoringInfoBuilder
    {
        private MultipleAlleleScoringInfo scoringInfo;

        public MultipleAlleleScoringInfoBuilder()
        {
            scoringInfo = new MultipleAlleleScoringInfo(
                new List<SingleAlleleScoringInfo>(),
                new List<SerologyEntry>());
        }
        
        public MultipleAlleleScoringInfoBuilder WithAlleleScoringInfos(IEnumerable<SingleAlleleScoringInfo> alleleScoringInfos)
        {
            scoringInfo = new MultipleAlleleScoringInfo(
                alleleScoringInfos,
                scoringInfo.MatchingSerologies);

            return this;
        }

        public MultipleAlleleScoringInfoBuilder WithMatchingSerologies(IEnumerable<SerologyEntry> matchingSerologies)
        {
            scoringInfo = new MultipleAlleleScoringInfo(
                scoringInfo.AlleleScoringInfos,
                matchingSerologies);

            return this;
        }

        public MultipleAlleleScoringInfo Build()
        {
            return scoringInfo;
        }
    }
}