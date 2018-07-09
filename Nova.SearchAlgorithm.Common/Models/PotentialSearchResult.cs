﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Nova.SearchAlgorithm.Common.Models
{
    public class PotentialSearchResult
    {
        private DonorResult donor;

        #region Partial donor information used in matching
        
        // Stored separately from the donors, as the lookup in the matches table only returns the id
        // We don't want to populate the full donor object until some filtering has been applied on those results
        public int DonorId { get; set; }
        // Stored separately from the Donor object as we don't want to populate all donor data until we're done filtering
        public PhenotypeInfo<IEnumerable<string>> DonorPGroups { get; set; }

        #endregion
        
        public DonorResult Donor
        {
            get
            {
                if (donor == null)
                {
                    throw new Exception("Attempted to access expanded donor information before it was populated");
                }

                return donor;
            }
            set
            {
                if (isMatchingDataFullyPopulated)
                {
                    throw new ReadOnlyException("Matching data cannot be changed after it has been marked as fully populated");
                }
                donor = value;
            }
        }

        public int TotalMatchCount => LocusMatchDetails.Where(m => m != null).Select(m => m.MatchCount).Sum();

        public int TypedLociCount => LocusMatchDetails.Count(m => m != null && m.IsLocusTyped);

        public int PopulatedLociCount => LocusMatchDetails.Count(m => m != null);
        
        #region Scoring Data
        
        public int MatchRank { get; set; }

        public int TotalMatchGrade { get; set; }

        public int TotalMatchConfidence { get; set; }

        #endregion
        
        private IEnumerable<LocusMatchDetails> LocusMatchDetails => new List<LocusMatchDetails>
        {
            MatchDetailsAtLocusA,
            MatchDetailsAtLocusB,
            MatchDetailsAtLocusC,
            MatchDetailsAtLocusDqb1,
            MatchDetailsAtLocusDrb1
        };

        private LocusMatchDetails MatchDetailsAtLocusA { get; set; }
        private LocusMatchDetails MatchDetailsAtLocusB { get; set; }
        private LocusMatchDetails MatchDetailsAtLocusC { get; set; }
        private LocusMatchDetails MatchDetailsAtLocusDrb1 { get; set; }
        private LocusMatchDetails MatchDetailsAtLocusDqb1 { get; set; }

        // As this class is populated gradually over time, this is used to indicate when we've populated all matching data we plan to
        // Until then, accessing certain null values will throw exceptions, on the assumption they are not yet populated
        private bool isMatchingDataFullyPopulated;

        public LocusMatchDetails MatchDetailsForLocus(Locus locus)
        {
            LocusMatchDetails matchDetails;
            switch (locus)
            {
                case Locus.A:
                    matchDetails = MatchDetailsAtLocusA;
                    break;
                case Locus.B:
                    matchDetails = MatchDetailsAtLocusB;
                    break;
                case Locus.C:
                    matchDetails = MatchDetailsAtLocusC;
                    break;
                case Locus.Dqb1:
                    matchDetails = MatchDetailsAtLocusDqb1;
                    break;
                case Locus.Drb1:
                    matchDetails = MatchDetailsAtLocusDrb1;
                    break;
                case Locus.Dpb1:
                    // TODO: NOVA-1300 implement matching for Dpb1
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (matchDetails == null && !isMatchingDataFullyPopulated)
            {
                throw new Exception($"Attempted to access match details for locus {locus} before they were generated");
            }

            return matchDetails;
        }
        
        public void SetMatchDetailsForLocus(Locus locus, LocusMatchDetails locusMatchDetails)
        {
            if (isMatchingDataFullyPopulated)
            {
                throw new ReadOnlyException("Matching data cannot be changed after it has been marked as fully populated");
            }
            
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
                case Locus.Dpb1:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void MarkMatchingDataFullyPopulated()
        {
            isMatchingDataFullyPopulated = true;
        }
    }
}