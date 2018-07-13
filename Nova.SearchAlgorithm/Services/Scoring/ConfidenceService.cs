﻿using System;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Common.Models.Scoring;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingDictionary;

namespace Nova.SearchAlgorithm.Services.Scoring
{
    public interface IConfidenceService
    {
        PhenotypeInfo<MatchConfidence> CalculateMatchConfidences(
            PhenotypeInfo<IHlaScoringLookupResult> donorScoringResults,
            PhenotypeInfo<IHlaScoringLookupResult> patientScoringResults,
            PhenotypeInfo<MatchGradeResult> matchGrades
        );
    }

    public class ConfidenceService : IConfidenceService
    {
        public PhenotypeInfo<MatchConfidence> CalculateMatchConfidences(
            PhenotypeInfo<IHlaScoringLookupResult> donorScoringResults,
            PhenotypeInfo<IHlaScoringLookupResult> patientScoringResults,
            PhenotypeInfo<MatchGradeResult> matchGrades
        )
        {
            // TODO: NOVA-1447: Implement
            throw new NotImplementedException();
        }
    }
}