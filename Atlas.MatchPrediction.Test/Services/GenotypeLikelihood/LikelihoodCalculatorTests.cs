﻿using System.Collections.Generic;
using System.Linq;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.MatchPrediction.Config;
using Atlas.MatchPrediction.Models;
using Atlas.MatchPrediction.Services.GenotypeLikelihood;
using Atlas.MatchPrediction.Test.TestHelpers.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace Atlas.MatchPrediction.Test.Services.GenotypeLikelihood
{
    public class LikelihoodCalculatorTests
    {
        private IGenotypeLikelihoodCalculator genotypeLikelihoodCalculator;

        [SetUp]
        public void SetUp()
        {
            genotypeLikelihoodCalculator = new GenotypeLikelihoodCalculator();
        }

        [TestCase(0, 16, 5.0, 10.0, 1600)]
        [TestCase(1, 8, 0.0, 5.0, 0)]
        [TestCase(2, 4, 0.123, 3.0, 2.952)]
        [TestCase(3, 2, 1.344, 3.0, 16.128)]
        [TestCase(4, 1, 1.345, 0.343, 0.92267)]
        public void CalculateLikelihood_WhenDiplotypesAreHeterozygous_ReturnsExpectedLikelihood(
            int numberOfHomozygousCases,
            int numberOfDiplotypes,
            decimal frequency1,
            decimal frequency2,
            decimal expectedLikelihood)
        {
            var hlaInfo1 = new LociInfo<string> {A = "A-1", B = "B-1", C = "C-1", Dqb1 = "Dqb1-1", Drb1 = "Drb1-1"};
            var hlaInfo2 = new LociInfo<string> {A = "A-2", B = "B-2", C = "C-2", Dqb1 = "Dqb1-2", Drb1 = "Drb1-2"};

            var lociToMakeHomozygous = LocusSettings.MatchPredictionLoci.Take(numberOfHomozygousCases);

            foreach (var locus in lociToMakeHomozygous)
            {
                hlaInfo1.SetLocus(locus, "homozygous");
                hlaInfo2.SetLocus(locus, "homozygous");
            }


            var homozygousImputedGenotype = new ImputedGenotype
            {
                Diplotypes = DiplotypeBuilder.New
                    .With(d => d.Item1, new Haplotype {Hla = hlaInfo1, Frequency = frequency1})
                    .With(d => d.Item2, new Haplotype {Hla = hlaInfo2, Frequency = frequency2})
                    .Build(numberOfDiplotypes),
                IsHomozygousAtEveryLocus = false
            };

            var actualLikelihood = genotypeLikelihoodCalculator.CalculateLikelihood(homozygousImputedGenotype);

            actualLikelihood.Should().Be(expectedLikelihood);
        }

        [TestCase(5.0, 25.0)]
        [TestCase(0, 0.0)]
        [TestCase(1.234, 1.522756)]
        public void CalculateLikelihood_ForSingleFullyHomozygousDiplotype_ReturnsExpectedLikelihood(
            decimal frequency,
            decimal expectedLikelihood)
        {
            var homozygousHlaData = new LociInfo<string>
            {
                A = "homozygous",
                B = "homozygous",
                C = "homozygous",
                Dqb1 = "homozygous",
                Drb1 = "homozygous"
            };

            var homozygousImputedGenotype = new ImputedGenotype
            {
                Diplotypes = new List<Diplotype>
                {
                    DiplotypeBuilder.New
                        .With(d => d.Item1, new Haplotype {Hla = homozygousHlaData, Frequency = frequency})
                        .With(d => d.Item2, new Haplotype {Hla = homozygousHlaData, Frequency = frequency})
                        .Build()
                },
                IsHomozygousAtEveryLocus = true
            };

            var actualLikelihood = genotypeLikelihoodCalculator.CalculateLikelihood(homozygousImputedGenotype);

            actualLikelihood.Should().Be(expectedLikelihood);
        }
    }
}