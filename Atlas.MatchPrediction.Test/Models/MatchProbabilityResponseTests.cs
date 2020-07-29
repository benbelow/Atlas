using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.Common.Utils.Models;
using Atlas.MatchPrediction.Test.TestHelpers.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace Atlas.MatchPrediction.Test.Models
{
    [TestFixture]
    public class MatchProbabilityResponseTests
    {
        [Test]
        public void Round_RoundsAllProbabilityValues()
        {
            const decimal unRounded = 0.1234567890m;
            const decimal rounded = 0.1235m;

            var unRoundedResponse = MatchProbabilityResponseBuilder.New.WithAllProbabilityValuesSetTo(unRounded).Build();

            var roundedResponse = unRoundedResponse.Round(4);

            roundedResponse.ZeroMismatchProbability.Decimal.Should().Be(rounded);
            roundedResponse.OneMismatchProbability.Decimal.Should().Be(rounded);
            roundedResponse.TwoMismatchProbability.Decimal.Should().Be(rounded);
            roundedResponse.ZeroMismatchProbabilityPerLocus.Should().BeEquivalentTo(new LociInfo<Probability>(new Probability(rounded)));
        }

        [Test]
        public void Round_WhenValuesNull_ReturnsNullValues()
        {
            var unRoundedResponse = MatchProbabilityResponseBuilder.New.WithAllProbabilityValuesNull().Build();

            var roundedResponse = unRoundedResponse.Round(4);

            roundedResponse.ZeroMismatchProbability.Should().BeNull();
            roundedResponse.OneMismatchProbability.Should().BeNull();
            roundedResponse.TwoMismatchProbability.Should().BeNull();
            roundedResponse.ZeroMismatchProbabilityPerLocus.Should().BeEquivalentTo(new LociInfo<Probability>(null));
        }
    }
}