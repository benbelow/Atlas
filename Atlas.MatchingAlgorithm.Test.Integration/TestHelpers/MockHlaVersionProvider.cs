﻿using Atlas.MatchingAlgorithm.Services.ConfigurationProviders;

namespace Atlas.MatchingAlgorithm.Test.Integration.TestHelpers
{
    // We don't care about this version info for any of the existing tests, and we don't want to faff around with setting up the data or allowing null responses in the live code.
    // So provide a trivial stubbed version of this class for tests.
    public class MockHlaVersionProvider : IActiveHlaVersionAccessor
    {
        public string GetActiveHlaNomenclatureVersion() => null;
    }
}
