﻿using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.Hla.Models;
using Atlas.HlaMetadataDictionary.Models.LookupEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.HlaMetadataDictionary.Models.Lookups.MatchingLookup
{
    internal class HlaMatchingLookupResult :
        IHlaMatchingLookupResult,
        IEquatable<HlaMatchingLookupResult>
    {
        public Locus Locus { get; }
        public string LookupName { get; }
        public TypingMethod TypingMethod { get; }
        public IEnumerable<string> MatchingPGroups { get; }
        public bool IsNullExpressingTyping => TypingMethod == TypingMethod.Molecular && !MatchingPGroups.Any();
        public object HlaInfoToSerialise => MatchingPGroups;

        internal HlaMatchingLookupResult(
            Locus locus,
            string lookupName,
            TypingMethod typingMethod,
            IEnumerable<string> matchingPGroups)
        {
            Locus = locus;
            LookupName = lookupName;
            TypingMethod = typingMethod;
            MatchingPGroups = matchingPGroups;
        }
        
        public bool Equals(HlaMatchingLookupResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return 
                Locus == other.Locus && 
                string.Equals(LookupName, other.LookupName) && 
                TypingMethod == other.TypingMethod && 
                MatchingPGroups.SequenceEqual(other.MatchingPGroups);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HlaMatchingLookupResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Locus;
                hashCode = (hashCode * 397) ^ LookupName.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) TypingMethod;
                hashCode = (hashCode * 397) ^ MatchingPGroups.GetHashCode();
                return hashCode;
            }
        }
    }
}
