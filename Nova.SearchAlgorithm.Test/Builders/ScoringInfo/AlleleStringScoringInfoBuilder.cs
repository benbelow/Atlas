﻿using System.Collections.Generic;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingDictionary;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingDictionary.ScoringLookup;

namespace Nova.SearchAlgorithm.Test.Builders.ScoringInfo
{
    public class AlleleStringScoringInfoBuilder
    {
        private AlleleStringScoringInfo scoringInfo;

        public AlleleStringScoringInfoBuilder()
        {
            scoringInfo = new AlleleStringScoringInfo(new List<SingleAlleleScoringInfo>());
        }
        
        public AlleleStringScoringInfoBuilder WithAlleleScoringInfos(IEnumerable<SingleAlleleScoringInfo> alleleScoringInfos)
        {
            scoringInfo = new AlleleStringScoringInfo(alleleScoringInfos);
            return this;
        }

        public AlleleStringScoringInfo Build()
        {
            return scoringInfo;
        }
    }
}