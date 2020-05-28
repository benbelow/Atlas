﻿using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.Hla.Models;
using Atlas.HlaMetadataDictionary.HlaTypingInfo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.HlaMetadataDictionary.Models.Lookups.AlleleNameLookup
{
    internal interface IAlleleNameLookupResult : ISerialisableHlaMetadata
    {
        List<string> CurrentAlleleNames { get; }
    }

    internal class AlleleNameLookupResult : 
        IAlleleNameLookupResult, 
        IEquatable<AlleleNameLookupResult>
    {
        public Locus Locus { get; }
        public string LookupName { get; }
        public TypingMethod TypingMethod => TypingMethod.Molecular;
        public List<string> CurrentAlleleNames { get; }
        public object HlaInfoToSerialise => CurrentAlleleNames.ToList(); //Needs to be reified for deserialisation Type validation

        public AlleleNameLookupResult(Locus locus, string lookupName, IEnumerable<string> currentAlleleNames)
        {
            Locus = locus;
            LookupName = lookupName;
            CurrentAlleleNames = currentAlleleNames.ToList();
        }

        public AlleleNameLookupResult(string locus, string lookupName, string currentAlleleName)
        {
            Locus = HlaMetadataDictionaryLoci.GetLocusFromTypingLocusNameIfExists(TypingMethod.Molecular, locus);
            LookupName = lookupName;
            CurrentAlleleNames = new List<string>{currentAlleleName};
        }

        #region IEquatable
        public bool Equals(AlleleNameLookupResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return 
                Locus == other.Locus && 
                string.Equals(LookupName, other.LookupName) && 
                CurrentAlleleNames.SequenceEqual(other.CurrentAlleleNames);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AlleleNameLookupResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Locus;
                hashCode = (hashCode * 397) ^ LookupName.GetHashCode();
                hashCode = (hashCode * 397) ^ CurrentAlleleNames.GetHashCode();
                return hashCode;
            }
        }
        #endregion
    }
}
