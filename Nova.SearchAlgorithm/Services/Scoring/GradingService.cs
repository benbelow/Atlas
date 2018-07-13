﻿using Nova.SearchAlgorithm.Client.Models;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Common.Models.Scoring;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingDictionary;

namespace Nova.SearchAlgorithm.Services.Scoring
{
    public interface IGradingService
    {
        PhenotypeInfo<MatchGradeResult> CalculateGrades(
            PhenotypeInfo<IHlaScoringLookupResult> donorScoringResults,
            PhenotypeInfo<IHlaScoringLookupResult> patientScoringResults
        );
    }

    public class GradingService : IGradingService
    {
        public PhenotypeInfo<MatchGradeResult> CalculateGrades(
            PhenotypeInfo<IHlaScoringLookupResult> donorScoringResults,
            PhenotypeInfo<IHlaScoringLookupResult> patientScoringResults
        )
        {
            // TODO: NOVA-1446: Implement
            throw new System.NotImplementedException();
        }
    }
}