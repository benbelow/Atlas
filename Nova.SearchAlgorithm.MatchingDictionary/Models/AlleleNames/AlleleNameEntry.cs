﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nova.SearchAlgorithm.MatchingDictionary.HlaTypingInfo;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;

namespace Nova.SearchAlgorithm.MatchingDictionary.Models.AlleleNames
{
    public class AlleleNameEntry : IEquatable<AlleleNameEntry>
    {
        public MatchLocus MatchLocus { get; }
        public string LookupName { get; }
        public IEnumerable<string> CurrentAlleleNames { get; }

        public AlleleNameEntry(MatchLocus matchLocus, string lookupName, IEnumerable<string> currentAlleleNames)
        {
            MatchLocus = matchLocus;
            LookupName = lookupName;
            CurrentAlleleNames = currentAlleleNames;
        }

        public AlleleNameEntry(string locus, string lookupName, string currentAlleleName)
            : this(
                  PermittedLocusNames.GetMatchLocusNameFromTypingLocusIfExists(TypingMethod.Molecular, locus),
                  lookupName,
                  new[] { currentAlleleName })
        {
        }

        public bool Equals(AlleleNameEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return 
                MatchLocus == other.MatchLocus && 
                string.Equals(LookupName, other.LookupName) && 
                CurrentAlleleNames.SequenceEqual(other.CurrentAlleleNames);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AlleleNameEntry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) MatchLocus;
                hashCode = (hashCode * 397) ^ LookupName.GetHashCode();
                hashCode = (hashCode * 397) ^ CurrentAlleleNames.GetHashCode();
                return hashCode;
            }
        }
    }
}
