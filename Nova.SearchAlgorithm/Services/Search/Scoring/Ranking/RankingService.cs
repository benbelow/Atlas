﻿using System.Collections.Generic;
using System.Linq;
using Nova.SearchAlgorithm.Common.Models.SearchResults;

namespace Nova.SearchAlgorithm.Services.Scoring.Ranking
{
    public interface IRankingService
    {
        /// <summary>
        /// Combines match count with scores for match grade and confidence, to order the search results
        /// </summary>
        IEnumerable<MatchAndScoreResult> RankSearchResults(IEnumerable<MatchAndScoreResult> results);
    }

    public class RankingService : IRankingService
    {
        public IEnumerable<MatchAndScoreResult> RankSearchResults(IEnumerable<MatchAndScoreResult> results)
        {
            return results
                .OrderByDescending(r => r.ScoreResult.MatchCount)
                .ThenByDescending(r => r.ScoreResult.GradeScore)
                .ThenByDescending(r => r.ScoreResult.ConfidenceScore);
        }
    }
}