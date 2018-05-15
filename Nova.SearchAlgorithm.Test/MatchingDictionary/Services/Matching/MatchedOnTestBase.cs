﻿using System.Collections.Generic;
using System.Linq;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypes;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingTypes;

namespace Nova.SearchAlgorithm.Test.MatchingDictionary.Services.Matching
{
    public class MatchedOnTestBase<TMatchedOn> where TMatchedOn : IMatchedOn
    {
        protected List<TMatchedOn> MatchingTypes;

        public MatchedOnTestBase(IEnumerable<TMatchedOn> matchingTypes)
        {
            MatchingTypes = matchingTypes.ToList();
        }

        protected TMatchedOn GetSingleMatchingType(MatchLocus matchLocus, string hlaName)
        {
            return MatchingTypes.Single(m =>
                m.HlaType.MatchLocus.Equals(matchLocus) && m.HlaType.Name.Equals(hlaName));
        }
    }
}
