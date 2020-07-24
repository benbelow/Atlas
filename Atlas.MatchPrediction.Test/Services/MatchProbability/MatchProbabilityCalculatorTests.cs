﻿using System.Collections.Generic;
using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.Common.Utils.Extensions;
using Atlas.MatchPrediction.Models;
using Atlas.MatchPrediction.Services.MatchProbability;
using Atlas.MatchPrediction.Test.TestHelpers.Builders;
using FluentAssertions;
using NUnit.Framework;
using static Atlas.Common.Test.SharedTestHelpers.Builders.DictionaryBuilder;

namespace Atlas.MatchPrediction.Test.Services.MatchProbability
{
    [TestFixture]
    public class MatchProbabilityCalculatorTests
    {
        private IMatchProbabilityCalculator matchProbabilityCalculator;

        private readonly PhenotypeInfo<string> defaultDonorHla1 = new PhenotypeInfo<string>("donor-hla-1");
        private readonly PhenotypeInfo<string> defaultDonorHla2 = new PhenotypeInfo<string>("donor-hla-2");
        private readonly PhenotypeInfo<string> defaultPatientHla1 = new PhenotypeInfo<string>("patient-hla-1");
        private readonly PhenotypeInfo<string> defaultPatientHla2 = new PhenotypeInfo<string>("patient-hla-2");

        private static readonly ISet<Locus> AllowedLoci = new HashSet<Locus> { Locus.A, Locus.B, Locus.C, Locus.Dqb1, Locus.Drb1 };

        [SetUp]
        public void Setup()
        {
            matchProbabilityCalculator = new MatchProbabilityCalculator();
        }

        [Test]
        public void CalculateMatchProbability_ReturnsMatchProbability()
        {
            var matchingPairs = new HashSet<GenotypeMatchDetails>
            {
                GenotypeMatchDetailsBuilder.New
                    .WithGenotypes(defaultDonorHla1, defaultPatientHla1)
                    .WithMatchCounts(new MatchCountsBuilder().TenOutOfTen().Build())
                    .WithAvailableLoci(AllowedLoci)
                    .Build(),
                GenotypeMatchDetailsBuilder.New
                    .WithGenotypes(defaultDonorHla2, defaultPatientHla2)
                    .WithMatchCounts(new MatchCountsBuilder().TenOutOfTen().WithDoubleMismatchAt(Locus.Dqb1, Locus.Drb1).Build())
                    .WithAvailableLoci(AllowedLoci)
                    .Build(),
            };

            var likelihoods = DictionaryWithCommonValue(0.5m, defaultDonorHla1, defaultDonorHla2, defaultPatientHla1, defaultPatientHla2);

            var actualProbability = matchProbabilityCalculator.CalculateMatchProbability(
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(likelihoods).WithGenotypes(defaultPatientHla1, defaultPatientHla2).Build(),
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(likelihoods).WithGenotypes(defaultDonorHla1, defaultDonorHla2).Build(),
                matchingPairs,
                AllowedLoci
            );

            var expectedMatchProbabilityPerLocus = new LociInfo<decimal?> {A = 0.5M, B = 0.5M, C = 0.5M, Dpb1 = null, Dqb1 = 0.25M, Drb1 = 0.25M};
            actualProbability.ZeroMismatchProbability.Decimal.Should().Be(0.25m);
            actualProbability.ZeroMismatchProbabilityPerLocus.ToDecimals().Should().Be(expectedMatchProbabilityPerLocus);
        }

        [Test]
        public void CalculateMatchProbability_WhenLocusWithOneMismatch_ReturnsMatchProbability()
        {
            var matchingPairs = new HashSet<GenotypeMatchDetails>
            {
                GenotypeMatchDetailsBuilder.New
                    .WithGenotypes(defaultDonorHla1, defaultPatientHla1)
                    .WithMatchCounts(new MatchCountsBuilder().TenOutOfTen().Build())
                    .WithAvailableLoci(AllowedLoci)
                    .Build(),
                GenotypeMatchDetailsBuilder.New
                    .WithGenotypes(defaultDonorHla2, defaultPatientHla2)
                    .WithMatchCounts(new MatchCountsBuilder().TenOutOfTen().WithSingleMismatchAt(Locus.Drb1).Build())
                    .WithAvailableLoci(AllowedLoci)
                    .Build(),
            };

            var likelihoods = DictionaryWithCommonValue(0.5m, defaultDonorHla1, defaultDonorHla2, defaultPatientHla1, defaultPatientHla2);

            var actualProbability = matchProbabilityCalculator.CalculateMatchProbability(
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(likelihoods).WithGenotypes(defaultPatientHla1, defaultPatientHla2).Build(),
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(likelihoods).WithGenotypes(defaultDonorHla1, defaultDonorHla2).Build(),
                matchingPairs,
                AllowedLoci
            );

            var expectedMatchProbabilityPerLocus = new LociInfo<decimal?> {A = 0.5M, B = 0.5M, C = 0.5M, Dpb1 = null, Dqb1 = 0.5M, Drb1 = 0.25M};
            actualProbability.OneMismatchProbability.Decimal.Should().Be(0.25m);
            actualProbability.ZeroMismatchProbabilityPerLocus.ToDecimals().Should().Be(expectedMatchProbabilityPerLocus);
        }

        [Test]
        public void CalculateMatchProbability_WhenLocusWithTwoMismatchesAtSameLocus_ReturnsMatchProbability()
        {
            var matchingPairs = new HashSet<GenotypeMatchDetails>
            {
                GenotypeMatchDetailsBuilder.New
                    .WithGenotypes(defaultDonorHla1, defaultPatientHla1)
                    .WithMatchCounts(new MatchCountsBuilder().TenOutOfTen().Build())
                    .WithAvailableLoci(AllowedLoci)
                    .Build(),
                GenotypeMatchDetailsBuilder.New
                    .WithGenotypes(defaultDonorHla2, defaultPatientHla2)
                    .WithMatchCounts(new MatchCountsBuilder().TenOutOfTen().WithDoubleMismatchAt(Locus.Drb1).Build())
                    .WithAvailableLoci(AllowedLoci)
                    .Build(),
            };

            var likelihoods = DictionaryWithCommonValue(0.5m, defaultDonorHla1, defaultDonorHla2, defaultPatientHla1, defaultPatientHla2);

            var actualProbability = matchProbabilityCalculator.CalculateMatchProbability(
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(likelihoods).WithGenotypes(defaultPatientHla1, defaultPatientHla2).Build(),
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(likelihoods).WithGenotypes(defaultDonorHla1, defaultDonorHla2).Build(),
                matchingPairs,
                AllowedLoci
            );

            var expectedMatchProbabilityPerLocus = new LociInfo<decimal?> {A = 0.5M, B = 0.5M, C = 0.5M, Dpb1 = null, Dqb1 = 0.5M, Drb1 = 0.25M};
            actualProbability.TwoMismatchProbability.Decimal.Should().Be(0.25m);
            actualProbability.ZeroMismatchProbabilityPerLocus.ToDecimals().Should().Be(expectedMatchProbabilityPerLocus);
        }

        [Test]
        public void CalculateMatchProbability_WhenLocusWithTwoMismatchesAtDifferentLoci_ReturnsMatchProbability()
        {
            var matchingPairs = new HashSet<GenotypeMatchDetails>
            {
                GenotypeMatchDetailsBuilder.New
                    .WithGenotypes(defaultDonorHla1, defaultPatientHla1)
                    .WithMatchCounts(new MatchCountsBuilder().TenOutOfTen().Build())
                    .WithAvailableLoci(AllowedLoci)
                    .Build(),
                GenotypeMatchDetailsBuilder.New
                    .WithGenotypes(defaultDonorHla2, defaultPatientHla2)
                    .WithMatchCounts(new MatchCountsBuilder().TenOutOfTen().WithSingleMismatchAt(Locus.B, Locus.C).Build())
                    .WithAvailableLoci(AllowedLoci)
                    .Build(),
            };

            var likelihoods = DictionaryWithCommonValue(0.5m, defaultDonorHla1, defaultDonorHla2, defaultPatientHla1, defaultPatientHla2);

            var actualProbability = matchProbabilityCalculator.CalculateMatchProbability(
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(likelihoods).WithGenotypes(defaultPatientHla1, defaultPatientHla2).Build(),
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(likelihoods).WithGenotypes(defaultDonorHla1, defaultDonorHla2).Build(),
                matchingPairs,
                AllowedLoci
            );

            var expectedMatchProbabilityPerLocus = new LociInfo<decimal?> {A = 0.5M, B = 0.25M, C = 0.25M, Dpb1 = null, Dqb1 = 0.5M, Drb1 = 0.5M};
            actualProbability.TwoMismatchProbability.Decimal.Should().Be(0.25m);
            actualProbability.ZeroMismatchProbabilityPerLocus.ToDecimals().Should().Be(expectedMatchProbabilityPerLocus);
        }

        [Test]
        public void CalculateMatchProbability_WhenPatientAndDonorHaveDifferentLikelihoodsForSameGenotypes_UsesCorrectLikelihoods()
        {
            var sharedHlaMatch = new PhenotypeInfo<string>("shared-hla-match");
            var sharedHlaMismatch = new PhenotypeInfo<string>("shared-hla-mismatch");

            var matchingPairs = new HashSet<GenotypeMatchDetails>
            {
                GenotypeMatchDetailsBuilder.New
                    .WithGenotypes(sharedHlaMatch, sharedHlaMatch)
                    .WithMatchCounts(new MatchCountsBuilder().TenOutOfTen().Build())
                    .WithAvailableLoci(AllowedLoci)
                    .Build()
            };

            var donorLikelihoods = new Dictionary<PhenotypeInfo<string>, decimal> {{sharedHlaMatch, 0.01m}, {sharedHlaMismatch, 0.02m}};
            var patientLikelihoods = new Dictionary<PhenotypeInfo<string>, decimal> {{sharedHlaMatch, 0.03m}, {sharedHlaMismatch, 0.04m}};

            var actualProbability = matchProbabilityCalculator.CalculateMatchProbability(
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(donorLikelihoods).WithGenotypes(sharedHlaMatch, sharedHlaMismatch).Build(),
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(patientLikelihoods).WithGenotypes(sharedHlaMatch, sharedHlaMismatch).Build(),
                matchingPairs,
                AllowedLoci
            );

            actualProbability.ZeroMismatchProbability.Decimal.Should().Be(0.1428571428571428571428571429m);
        }

        [Test]
        public void CalculateMatchProbability_WithUnrepresentedPhenotypes_HasNullProbability()
        {
            var likelihoods = DictionaryWithCommonValue(0m, defaultDonorHla1, defaultDonorHla2, defaultPatientHla1, defaultPatientHla2);

            var actualProbability = matchProbabilityCalculator.CalculateMatchProbability(
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(likelihoods).WithGenotypes(defaultPatientHla1, defaultPatientHla2).Build(),
                SubjectCalculatorInputsBuilder.New.WithLikelihoods(likelihoods).WithGenotypes(defaultDonorHla1, defaultDonorHla2).Build(),
                new HashSet<GenotypeMatchDetails>(),
                AllowedLoci
            );

            actualProbability.ZeroMismatchProbability.Decimal.Should().Be(null);
            actualProbability.OneMismatchProbability.Decimal.Should().Be(null);
            actualProbability.TwoMismatchProbability.Decimal.Should().Be(null);
        }
    }
}