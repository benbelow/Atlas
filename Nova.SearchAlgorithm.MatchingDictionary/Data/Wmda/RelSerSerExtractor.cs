﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Wmda;
using Nova.SearchAlgorithm.MatchingDictionary.Repositories;

namespace Nova.SearchAlgorithm.MatchingDictionary.Data.Wmda
{
    internal class RelSerSerExtractor : IWmdaDataExtractor
    {
        public IEnumerable<IWmdaHlaTyping> ExtractData(IWmdaRepository repo)
        {
            var data = new List<RelSerSer>();

            foreach (var line in repo.RelSerSer)
            {
                var matched = new Regex(@"(\w+)\;(\d*)\;([\d\/]*)\;([\d\/]*)").Match(line).Groups;
                data.Add(new RelSerSer(
                    matched[1].Value,
                    matched[2].Value,
                    !matched[3].Value.Equals("") ? matched[3].Value.Split('/') : new string[] { },
                    !matched[4].Value.Equals("") ? matched[4].Value.Split('/') : new string[] { }
                ));
            }

            return data;
        }
    }
}
