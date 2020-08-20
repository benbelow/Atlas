﻿using Atlas.MatchPrediction.ExternalInterface.Models;
using LochNessBuilder;

namespace Atlas.MatchPrediction.Test.Verification.VerificationFrameworkTests.TestHelpers
{
    [Builder]
    internal static class HaplotypeFrequencyBuilder
    {
        private const string DefaultHla = "hla";

        public static Builder<HaplotypeFrequencyFile> Default =>
            Builder<HaplotypeFrequencyFile>.New
                .With(x => x.A, DefaultHla)
                .With(x => x.B, DefaultHla)
                .With(x => x.C, DefaultHla)
                .With(x => x.Dqb1, DefaultHla)
                .With(x => x.Drb1, DefaultHla)
                .With(x => x.Frequency, 0.0000001m);
    }
}
