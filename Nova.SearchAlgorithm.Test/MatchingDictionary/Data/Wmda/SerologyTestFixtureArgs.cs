﻿using Nova.SearchAlgorithm.MatchingDictionary.Models.Wmda.Filters;

namespace Nova.SearchAlgorithm.Test.MatchingDictionary.Data.Wmda
{
    public class SerologyTestFixtureArgs
    {
        public static object[] Args = {
            new object[] { SerologyFilter.Instance.Filter, new[] { "A", "B", "Cw", "DQ", "DR" } }
        };
    }
}
