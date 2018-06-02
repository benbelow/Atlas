﻿using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingTypings;
using System.Collections.Generic;
using System.Linq;

namespace Nova.SearchAlgorithm.MatchingDictionary.Services.Matching
{
    internal class SerologyToAlleleMapper
    {
        public IEnumerable<IAlleleInfoForMatching> GetAlleleMappingsForSerology(IHlaInfoToMapSerologyToAllele hlaInfo, ISerologyInfoForMatching serologyInfo)
        {
            var matchLocus = serologyInfo.HlaTyping.MatchLocus;
            var matchingSerologies = serologyInfo.MatchingSerologies.Select(m => m.Name);

            return
                from alleleInfo in hlaInfo.AlleleInfoForMatching
                join alleleToSerology in hlaInfo.AlleleToSerologyRelationships
                    on new { WmdaLocus = alleleInfo.TypingUsedInMatching.Locus, alleleInfo.TypingUsedInMatching.Name }
                    equals new { WmdaLocus = alleleToSerology.Locus, alleleToSerology.Name }
                where alleleInfo.TypingUsedInMatching.MatchLocus.Equals(matchLocus)
                      && alleleToSerology.Serologies.Intersect(matchingSerologies).Any()
                select alleleInfo;
        }
    }
}
