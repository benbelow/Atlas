﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;

namespace Nova.SearchAlgorithm.MatchingDictionary.Models.Wmda
{
    public class RelDnaSer : IWmdaHlaTyping, IEquatable<RelDnaSer>
    {
        public TypingMethod TypingMethod => TypingMethod.Molecular;
        public string Locus { get; set; }
        public string Name { get; set; }
        public IEnumerable<SerologyAssignment> Assignments { get; }
        public IEnumerable<string> Serologies { get; }

        public RelDnaSer(string wmdaLocus, string name, IEnumerable<SerologyAssignment> assignments)
        {
            Locus = wmdaLocus;
            Name = name;
            Assignments = assignments;
            Serologies = Assignments.Select(a => a.Name).Distinct().OrderBy(s => s);
        }

        public override string ToString()
        {
            return $"locus: {Locus}, allele: {Name}, assignments: {string.Join("/", Assignments)}";
        }

        public bool Equals(RelDnaSer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return 
                string.Equals(Locus, other.Locus) 
                && string.Equals(Name, other.Name) 
                && Assignments.SequenceEqual(other.Assignments);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RelDnaSer) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Locus.GetHashCode();
                hashCode = (hashCode * 397) ^ Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Assignments.GetHashCode();
                return hashCode;
            }
        }
    }
}
