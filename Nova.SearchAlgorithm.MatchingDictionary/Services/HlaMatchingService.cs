﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nova.SearchAlgorithm.MatchingDictionary.Data.Wmda;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingTypes;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Wmda;
using Nova.SearchAlgorithm.MatchingDictionary.Repositories;
using Nova.SearchAlgorithm.MatchingDictionary.Services.Matching;

namespace Nova.SearchAlgorithm.MatchingDictionary.Services
{
    public interface IHlaMatchingService
    {
        IEnumerable<IMatchedHla> MatchAllHla(
            Func<IWmdaHlaType, bool> serologyFilter, Func<IWmdaHlaType, bool> molecularFilter);
    }

    public class HlaMatchingService : IHlaMatchingService
    {
        private readonly IWmdaRepository _repository;

        public HlaMatchingService(IWmdaRepository repo)
        {
            _repository = repo;
        }

        public IEnumerable<IMatchedHla> MatchAllHla(
            Func<IWmdaHlaType, bool> serologyFilter, Func<IWmdaHlaType, bool> molecularFilter)
        {
            var matchedHlaSourceData = new MatchedHlaSourceData
            {
                AlleleToPGroups =
                    new AlleleToPGroupMatcher().MatchAllelesToPGroups(_repository, molecularFilter).ToList(),
                SerologyToSerology = new SerologyToSerologyMatcher()
                    .MatchSerologyToSerology(_repository, serologyFilter).ToList(),
                RelDnaSer = WmdaDataFactory
                    .GetData<RelDnaSer>(_repository, molecularFilter).ToList()
            };

            var matchedAlleles = new AlleleToSerologyMatcher().MatchAllelesToSerology(matchedHlaSourceData);
            var matchedSerology = new SerologyToPGroupsMatcher().MatchSerologyToPGroups(matchedHlaSourceData);

            var allMatchingHla = new List<IMatchedHla>();
            allMatchingHla.AddRange(matchedAlleles);
            allMatchingHla.AddRange(matchedSerology);

            return allMatchingHla;
        }        
    }
}
