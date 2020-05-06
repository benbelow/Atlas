﻿using Atlas.HlaMetadataDictionary.Models.HLATypings;
using Atlas.HlaMetadataDictionary.Models.MatchingTypings;
using Atlas.HlaMetadataDictionary.Models.Wmda;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.HlaMetadataDictionary.Services.HlaMatchPreCalculation.SerologyRelationships
{
    internal class BroadCalculator: MatchingSerologyCalculatorBase
    {
        public BroadCalculator(IEnumerable<RelSerSer> serologyRelationships) :
            base(serologyRelationships)
        {
        }

        /// <summary>
        /// Broad antigens will have 2+ split children, 0+ associated children, and 0+ associated grandchildren.
        /// </summary>
        protected override IEnumerable<MatchingSerology> GetIndirectlyMatchingSerologies(SerologyFamily family)
        {
            var splits = family.Child.SplitAntigens
                .Select(s => new HlaNom(TypingMethod.Serology, family.Child.TypingLocus, s))
                .ToList();

            var splitsOfBroad = splits.Select(GetSplitMatchingSerology);

            var associatedToSplits = splits.SelectMany(split =>
                GetAssociatedMatchingSerologies(SerologyFamily.GetChild(SerologyRelationships, split)));

            var associatedToBroad = GetAssociatedMatchingSerologies(family.Child);

            return splitsOfBroad
                .Concat(associatedToSplits)
                .Concat(associatedToBroad);
        }
    }
}
