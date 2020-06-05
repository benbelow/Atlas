﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.Common.Test.SharedTestHelpers.Builders;
using Atlas.MatchPrediction.Client.Models.GenotypeLikelihood;
using Atlas.MatchPrediction.Data.Models;
using Atlas.MatchPrediction.Services.GenotypeLikelihood;
using Atlas.MatchPrediction.Services.HaplotypeFrequencies;
using Atlas.MatchPrediction.Test.Integration.TestHelpers.Builders.FrequencySetFile;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Atlas.MatchPrediction.Test.Integration.IntegrationTests.GenotypeLikelihood
{
    ///<summary>
    /// These tests are snapshots based on some manually calculated frequencies/expectations.
    /// Any tests of the GenotypeLikelihood calculator that are not such tests should be added elsewhere.
    ///</summary>
    [TestFixture]
    public class GenotypeLikelihoodTests
    {
        private IFrequencySetService importService;
        private IGenotypeLikelihoodService likelihoodService;

        private const string A1 = "A1:A1";
        private const string A2 = "A2:A2";
        private const string B1 = "B1:B1";
        private const string B2 = "B2:B2";
        private const string C1 = "C1:C1";
        private const string C2 = "C2:C2";
        private const string Dqb11 = "Dqb11:Dqb11";
        private const string Dqb12 = "Dqb12:Dqb12";
        private const string Drb11 = "Drb11:Drb11";
        private const string Drb12 = "Drb12:Drb12";
        
        [SetUp]
        public async Task SetUp()
        {
            importService = DependencyInjection.DependencyInjection.Provider.GetService<IFrequencySetService>();
            likelihoodService = DependencyInjection.DependencyInjection.Provider.GetService<IGenotypeLikelihoodService>();

            // 32 possible haplotypes for a single unambiguous genotype.
            var allPossibleHaplotypes = new List<HaplotypeFrequency>
            {
                new HaplotypeFrequency {A = A2, B = B1, C = C1, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.32m},
                new HaplotypeFrequency {A = A1, B = B2, C = C2, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.31m},
                new HaplotypeFrequency {A = A2, B = B1, C = C1, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.301m},
                new HaplotypeFrequency {A = A1, B = B2, C = C2, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.29m},
                new HaplotypeFrequency {A = A2, B = B1, C = C1, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.28m},
                new HaplotypeFrequency {A = A1, B = B2, C = C2, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.27m},
                new HaplotypeFrequency {A = A2, B = B1, C = C1, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.26m},
                new HaplotypeFrequency {A = A1, B = B2, C = C2, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.25m},
                new HaplotypeFrequency {A = A2, B = B1, C = C2, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.24m},
                new HaplotypeFrequency {A = A1, B = B2, C = C1, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.23m},
                new HaplotypeFrequency {A = A2, B = B1, C = C2, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.22m},
                new HaplotypeFrequency {A = A1, B = B2, C = C1, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.21m},
                new HaplotypeFrequency {A = A2, B = B1, C = C2, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.201m},
                new HaplotypeFrequency {A = A1, B = B2, C = C1, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.19m},
                new HaplotypeFrequency {A = A2, B = B1, C = C2, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.18m},
                new HaplotypeFrequency {A = A1, B = B2, C = C1, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.17m},
                new HaplotypeFrequency {A = A2, B = B2, C = C1, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.16m},
                new HaplotypeFrequency {A = A1, B = B1, C = C2, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.15m},
                new HaplotypeFrequency {A = A2, B = B2, C = C1, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.14m},
                new HaplotypeFrequency {A = A1, B = B1, C = C2, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.13m},
                new HaplotypeFrequency {A = A2, B = B2, C = C1, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.12m},
                new HaplotypeFrequency {A = A1, B = B1, C = C2, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.11m},
                new HaplotypeFrequency {A = A2, B = B2, C = C1, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.101m},
                new HaplotypeFrequency {A = A1, B = B1, C = C2, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.9m},
                new HaplotypeFrequency {A = A2, B = B2, C = C2, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.8m},
                new HaplotypeFrequency {A = A1, B = B1, C = C1, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.7m},
                new HaplotypeFrequency {A = A2, B = B2, C = C2, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.6m},
                new HaplotypeFrequency {A = A1, B = B1, C = C1, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.5m},
                new HaplotypeFrequency {A = A2, B = B2, C = C2, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.4m},
                new HaplotypeFrequency {A = A1, B = B1, C = C1, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.3m},
                new HaplotypeFrequency {A = A2, B = B2, C = C2, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.2m},
                new HaplotypeFrequency {A = A1, B = B1, C = C1, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.1m}
            };

            await ImportFrequencies(allPossibleHaplotypes);
        }

        [Test]
        public async Task CalculateLikelihood_WhenAllLociAreHeterozygous_ReturnsExpectedLikelihood()
        {
            var genotypeInput = new GenotypeLikelihoodInput {Genotype = PhenotypeInfoBuilder.New.Build()};
            const decimal expectedLikelihood = 3.28716m;

            var likelihoodResponse = await likelihoodService.CalculateLikelihood(genotypeInput);

            likelihoodResponse.Likelihood.Should().Be(expectedLikelihood);
        }

        [TestCase(Locus.A, 1.4166)]
        [TestCase(Locus.B, 1.41646)]
        [TestCase(Locus.C, 0.92462)]
        [TestCase(Locus.Dqb1, 1.3765)]
        [TestCase(Locus.Drb1, 1.63234)]
        public async Task CalculateLikelihood_WhenLocusIsHomozygous_ReturnsExpectedLikelihood(Locus homozygousLocus, decimal expectedLikelihood)
        {
            var genotype = PhenotypeInfoBuilder.New
                .With(d => d.A, new LocusInfo<string> {Position1 = A1, Position2 = A2})
                .With(d => d.B, new LocusInfo<string> {Position1 = B1, Position2 = B2})
                .With(d => d.C, new LocusInfo<string> {Position1 = C1, Position2 = C2})
                .With(d => d.Dqb1, new LocusInfo<string> {Position1 = Dqb11, Position2 = Dqb12})
                .With(d => d.Drb1, new LocusInfo<string> {Position1 = Drb11, Position2 = Drb12})
                .Build();

            genotype.SetPosition(homozygousLocus, LocusPosition.Two,
                genotype.GetPosition(homozygousLocus, LocusPosition.One));

            var genotypeInput = new GenotypeLikelihoodInput {Genotype = genotype};

            var likelihoodResponse = await likelihoodService.CalculateLikelihood(genotypeInput);

            likelihoodResponse.Likelihood.Should().Be(expectedLikelihood);
        }

        [TestCase(new[] {Locus.A, Locus.C}, 0.6)]
        [TestCase(new[] {Locus.B, Locus.Drb1}, 0.8674)]
        [TestCase(new[] {Locus.C, Locus.Dqb1, Locus.B}, 0.2522)]
        [TestCase(new[] {Locus.Dqb1, Locus.A, Locus.Drb1, Locus.C}, 0.034)]
        public async Task CalculateLikelihood_WhenMultipleLocusAreHomozygous_ReturnsExpectedLikelihood(
            Locus[] homozygousLoci,
            decimal expectedLikelihood)
        {
            var genotype = PhenotypeInfoBuilder.New
                .With(d => d.A, new LocusInfo<string> {Position1 = A1, Position2 = A2})
                .With(d => d.B, new LocusInfo<string> {Position1 = B1, Position2 = B2})
                .With(d => d.C, new LocusInfo<string> {Position1 = C1, Position2 = C2})
                .With(d => d.Dqb1, new LocusInfo<string> {Position1 = Dqb11, Position2 = Dqb12})
                .With(d => d.Drb1, new LocusInfo<string> {Position1 = Drb11, Position2 = Drb12})
                .Build();

            foreach (var homozygousLocus in homozygousLoci)
            {
                genotype.SetPosition(homozygousLocus, LocusPosition.Two,
                    genotype.GetPosition(homozygousLocus, LocusPosition.One));
            }

            var genotypeInput = new GenotypeLikelihoodInput {Genotype = genotype};

            var likelihoodResponse = await likelihoodService.CalculateLikelihood(genotypeInput);

            likelihoodResponse.Likelihood.Should().Be(expectedLikelihood);
        }

        //TODO: ATLAS-345: This test will no longer be correct once we handle untyped input loci.
        [TestCase(Locus.C)]
        [TestCase(Locus.Dqb1)]
        public async Task CalculateLikelihood_WhenGenotypeHasNullLoci_ReturnsZeroLikelihood(Locus locusToBeNull)
        {
            var genotype = PhenotypeInfoBuilder.New
                .With(d => d.A, new LocusInfo<string> {Position1 = A1, Position2 = A2})
                .With(d => d.B, new LocusInfo<string> {Position1 = B1, Position2 = B2})
                .With(d => d.C, new LocusInfo<string> {Position1 = C1, Position2 = C2})
                .With(d => d.Dqb1, new LocusInfo<string> {Position1 = Dqb11, Position2 = Dqb12})
                .With(d => d.Drb1, new LocusInfo<string> {Position1 = Drb11, Position2 = Drb12})
                .Build();

            var genotypeInput = new GenotypeLikelihoodInput {Genotype = genotype};
            genotypeInput.Genotype.SetLocus(locusToBeNull, null);

            const decimal expectedLikelihood = 0;

            var likelihoodResponse = await likelihoodService.CalculateLikelihood(genotypeInput);

            likelihoodResponse.Likelihood.Should().Be(expectedLikelihood);
        }

        [TestCase(Locus.A)]
        [TestCase(Locus.B)]
        [TestCase(Locus.C)]
        [TestCase(Locus.Dqb1)]
        [TestCase(Locus.Drb1)]
        public async Task CalculateLikelihood_WhenNoHaplotypesAreRepresentedInDatabase_ReturnsZeroLikelihood(Locus unrepresentedLocus)
        {
            var genotype = PhenotypeInfoBuilder.New
                .With(d => d.A, new LocusInfo<string> {Position1 = A1, Position2 = A2})
                .With(d => d.B, new LocusInfo<string> {Position1 = B1, Position2 = B2})
                .With(d => d.C, new LocusInfo<string> {Position1 = C1, Position2 = C2})
                .With(d => d.Dqb1, new LocusInfo<string> {Position1 = Dqb11, Position2 = Dqb12})
                .With(d => d.Drb1, new LocusInfo<string> {Position1 = Drb11, Position2 = Drb12})
                .Build();

            var genotypeInput = new GenotypeLikelihoodInput {Genotype = genotype};
            genotypeInput.Genotype.SetLocus(unrepresentedLocus, "un-represented");

            const decimal expectedLikelihood = 0;

            var likelihoodResponse = await likelihoodService.CalculateLikelihood(genotypeInput);

            likelihoodResponse.Likelihood.Should().Be(expectedLikelihood);
        }

        [TestCase("ExtraField", Locus.A)]
        [TestCase("ExtraField:ExtraField", Locus.A)]
        [TestCase("ExtraField", Locus.B)]
        [TestCase("ExtraField:ExtraField", Locus.B)]
        [TestCase("ExtraField", Locus.C)]
        [TestCase("ExtraField:ExtraField", Locus.C)]
        [TestCase("ExtraField", Locus.Dqb1)]
        [TestCase("ExtraField:ExtraField", Locus.Dqb1)]
        [TestCase("ExtraField", Locus.Drb1)]
        [TestCase("ExtraField:ExtraField", Locus.Drb1)]
        public async Task CalculateLikelihood_WhenGenotypeHasThreeOrFourFieldAllele_ReturnsExpectedLikelihood(string fieldsToAdd, Locus locus)
        {
            var genotype = PhenotypeInfoBuilder.New
                .With(d => d.A, new LocusInfo<string> { Position1 = A1, Position2 = A2 })
                .With(d => d.B, new LocusInfo<string> { Position1 = B1, Position2 = B2 })
                .With(d => d.C, new LocusInfo<string> { Position1 = C1, Position2 = C2 })
                .With(d => d.Dqb1, new LocusInfo<string> { Position1 = Dqb11, Position2 = Dqb12 })
                .With(d => d.Drb1, new LocusInfo<string> { Position1 = Drb11, Position2 = Drb12 })
                .Build();

            genotype.SetPosition(locus, LocusPosition.One, $"{genotype.GetPosition(locus, LocusPosition.One)}:{fieldsToAdd}");
            genotype.SetPosition(locus, LocusPosition.Two, $"{genotype.GetPosition(locus, LocusPosition.Two)}:{fieldsToAdd}");

            var genotypeInput = new GenotypeLikelihoodInput {Genotype = genotype};
            const decimal expectedLikelihood = 3.28716m;

            var likelihoodResponse = await likelihoodService.CalculateLikelihood(genotypeInput);

            likelihoodResponse.Likelihood.Should().Be(expectedLikelihood);
        }

        [Test]
        public async Task CalculateLikelihood_WhenOnlySomeHaplotypesAreRepresentedInDatabase_ReturnsExpectedLikelihood()
        {
            // 16 of the possible 32 haplotypes for a single unambiguous genotype.
            var haplotypesWith16Missing = new List<HaplotypeFrequency>
            {
                new HaplotypeFrequency {A = A2, B = B1, C = C1, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.32m},
                new HaplotypeFrequency {A = A1, B = B2, C = C2, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.31m},
                new HaplotypeFrequency {A = A2, B = B1, C = C1, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.301m},
                new HaplotypeFrequency {A = A1, B = B2, C = C2, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.29m},
                new HaplotypeFrequency {A = A2, B = B1, C = C1, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.28m},
                new HaplotypeFrequency {A = A1, B = B2, C = C2, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.27m},
                new HaplotypeFrequency {A = A2, B = B1, C = C1, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.26m},
                new HaplotypeFrequency {A = A1, B = B2, C = C2, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.25m},
                new HaplotypeFrequency {A = A2, B = B1, C = C2, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.24m},
                new HaplotypeFrequency {A = A1, B = B2, C = C1, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.23m},
                new HaplotypeFrequency {A = A2, B = B1, C = C2, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.22m},
                new HaplotypeFrequency {A = A1, B = B2, C = C1, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.21m},
                new HaplotypeFrequency {A = A2, B = B1, C = C2, DQB1 = Dqb12, DRB1 = Drb11, Frequency = 0.201m},
                new HaplotypeFrequency {A = A1, B = B2, C = C1, DQB1 = Dqb11, DRB1 = Drb12, Frequency = 0.19m},
                new HaplotypeFrequency {A = A2, B = B1, C = C2, DQB1 = Dqb12, DRB1 = Drb12, Frequency = 0.18m},
                new HaplotypeFrequency {A = A1, B = B2, C = C1, DQB1 = Dqb11, DRB1 = Drb11, Frequency = 0.17m}
            };

            await ImportFrequencies(haplotypesWith16Missing);

            var genotypeInput = new GenotypeLikelihoodInput {Genotype = PhenotypeInfoBuilder.New.Build()};
            const decimal expectedLikelihood = 0.99456m;

            var likelihoodResponse = await likelihoodService.CalculateLikelihood(genotypeInput);

            likelihoodResponse.Likelihood.Should().Be(expectedLikelihood);
        }

        private async Task ImportFrequencies(IEnumerable<HaplotypeFrequency> haplotypes)
        {
            using var file = FrequencySetFileBuilder.New(haplotypes).Build();
            await importService.ImportFrequencySet(file);
        }
    }
}