﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Nova.SearchAlgorithm.Common.Models
{
    public class PotentialSearchResult
    {
        public DonorResult Donor { get; set; }

        public int TotalMatchCount
        {
            get { return LocusMatchDetails.Where(m => m != null).Select(m => m.MatchCount).Sum(); }
        }

        private IEnumerable<LocusMatchDetails> LocusMatchDetails => new List<LocusMatchDetails>
        {
            MatchDetailsAtLocusA,
            MatchDetailsAtLocusB,
            MatchDetailsAtLocusC,
            MatchDetailsAtLocusDqb1,
            MatchDetailsAtLocusDrb1
        };

        public int TypedLociCount { get; set; }
        public int MatchRank { get; set; }
        public int TotalMatchGrade { get; set; }
        public int TotalMatchConfidence { get; set; }
        public LocusMatchDetails MatchDetailsAtLocusA { get; set; }
        public LocusMatchDetails MatchDetailsAtLocusB { get; set; }
        public LocusMatchDetails MatchDetailsAtLocusC { get; set; }
        public LocusMatchDetails MatchDetailsAtLocusDrb1 { get; set; }
        public LocusMatchDetails MatchDetailsAtLocusDqb1 { get; set; }

        public LocusMatchDetails MatchDetailsForLocus(Locus locus)
        {
            switch (locus)
            {
                case Locus.A:
                    return MatchDetailsAtLocusA;
                case Locus.B:
                    return MatchDetailsAtLocusB;
                case Locus.C:
                    return MatchDetailsAtLocusC;
                case Locus.Dqb1:
                    return MatchDetailsAtLocusDqb1;
                case Locus.Drb1:
                    return MatchDetailsAtLocusDrb1;
                default:
                    throw new NotImplementedException();
            }
        }
        
        public void SetMatchDetailsForLocus(Locus locus, LocusMatchDetails locusMatchDetails)
        {
            switch (locus)
            {
                case Locus.A:
                    MatchDetailsAtLocusA = locusMatchDetails;
                    break;
                case Locus.B:
                    MatchDetailsAtLocusB = locusMatchDetails;
                    break;
                case Locus.C:
                    MatchDetailsAtLocusC = locusMatchDetails;
                    break;
                case Locus.Dqb1:
                    MatchDetailsAtLocusDqb1 = locusMatchDetails;
                    break;
                case Locus.Drb1:
                    MatchDetailsAtLocusDrb1 = locusMatchDetails;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}