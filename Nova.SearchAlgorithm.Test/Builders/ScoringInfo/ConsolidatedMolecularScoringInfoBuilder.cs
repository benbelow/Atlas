﻿using System.Collections.Generic;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups.ScoringLookup;

namespace Nova.SearchAlgorithm.Test.Builders.ScoringInfo
{
    public class ConsolidatedMolecularScoringInfoBuilder
    {
        private ConsolidatedMolecularScoringInfo scoringInfo;

        public ConsolidatedMolecularScoringInfoBuilder()
        {
            scoringInfo = new ConsolidatedMolecularScoringInfo(
                new List<string>(), 
                new List<string>(), 
                new List<SerologyEntry>()
                );
        }
        
        public ConsolidatedMolecularScoringInfoBuilder WithMatchingPGroups(IEnumerable<string> pGroups)
        {
            scoringInfo = new ConsolidatedMolecularScoringInfo(pGroups, scoringInfo.MatchingGGroups, scoringInfo.MatchingSerologies);
            return this;
        }
        
        public ConsolidatedMolecularScoringInfoBuilder WithMatchingGGroups(IEnumerable<string> gGroups)
        {
            scoringInfo = new ConsolidatedMolecularScoringInfo(scoringInfo.MatchingPGroups, gGroups, scoringInfo.MatchingSerologies);
            return this;
        }
        
        public ConsolidatedMolecularScoringInfoBuilder WithMatchingSerologies(IEnumerable<SerologyEntry> matchingSerologies)
        {
            scoringInfo = new ConsolidatedMolecularScoringInfo(scoringInfo.MatchingPGroups, scoringInfo.MatchingGGroups, matchingSerologies);
            return this;
        }

        public ConsolidatedMolecularScoringInfo Build()
        {
            return scoringInfo;
        }
    }
}