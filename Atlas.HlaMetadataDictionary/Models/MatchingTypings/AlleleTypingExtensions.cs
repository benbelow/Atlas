﻿using Atlas.HlaMetadataDictionary.Exceptions;
using Atlas.HlaMetadataDictionary.Models.HLATypings;
using Atlas.HlaMetadataDictionary.Models.Wmda;
using System;

namespace Atlas.HlaMetadataDictionary.Models.MatchingTypings
{
    internal static class AlleleTypingExtensions
    {
        public static AlleleTypingStatus ToAlleleTypingStatus(this AlleleStatus alleleStatus)
        {
            if (alleleStatus == null)
            {
                return AlleleTypingStatus.GetDefaultStatus();
            }

            if (!Enum.TryParse(alleleStatus.SequenceStatus, true, out SequenceStatus sequenceStatus))
            {
                throw new HlaMetadataDictionaryException(alleleStatus, $"Sequence status {alleleStatus.SequenceStatus} not recognised.");
            }

            if (!Enum.TryParse(alleleStatus.DnaCategory, true, out DnaCategory dnaCategory))
            {
                throw new HlaMetadataDictionaryException(alleleStatus, $"DNA category {alleleStatus.DnaCategory} not recognised.");
            }

            return new AlleleTypingStatus(sequenceStatus, dnaCategory);
        }
    }
}
