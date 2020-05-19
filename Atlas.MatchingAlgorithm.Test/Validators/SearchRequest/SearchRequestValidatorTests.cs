﻿using Atlas.MatchingAlgorithm.Client.Models;
using Atlas.MatchingAlgorithm.Client.Models.Donors;
using Atlas.MatchingAlgorithm.Client.Models.SearchRequests;
using Atlas.MatchingAlgorithm.Validators.SearchRequest;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Atlas.MatchingAlgorithm.Test.Validators.SearchRequest
{
    [TestFixture]
    public class SearchRequestValidatorTests
    {
        private SearchRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new SearchRequestValidator();
        }

        [Test]
        public void Validator_WhenMatchCriteriaMissing_ShouldHaveValidationError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.MatchCriteria, (MismatchCriteria) null);
        }

        [Test]
        public void Validator_WhenSearchTypeMissing_ShouldHaveValidationError()
        {
            var result = validator.Validate(new Client.Models.SearchRequests.SearchRequest
            {
                MatchCriteria = new MismatchCriteria()
            });
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void Validator_WithInvalidSearchType_ShouldHaveValidationError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.SearchType, (DonorType) 999);
        }

        [Test]
        public void Validator_WithMatchCriteriaForLocusCAndNoHlaDataAtC_ShouldHaveValidationError()
        {
            var result = validator.Validate(new Client.Models.SearchRequests.SearchRequest
            {
                SearchType = DonorType.Adult,
                MatchCriteria = new MismatchCriteria
                {
                    LocusMismatchA = new LocusMismatchCriteria(),
                    LocusMismatchB = new LocusMismatchCriteria(),
                    LocusMismatchDrb1 = new LocusMismatchCriteria(),
                    LocusMismatchC = new LocusMismatchCriteria(),
                },
                SearchHlaData = new SearchHlaData
                {
                    LocusSearchHlaA = new LocusSearchHla {SearchHla1 = "hla", SearchHla2 = "hla"},
                    LocusSearchHlaB = new LocusSearchHla {SearchHla1 = "hla", SearchHla2 = "hla"},
                    LocusSearchHlaDrb1 = new LocusSearchHla {SearchHla1 = "hla", SearchHla2 = "hla"},
                }
            });
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void Validator_WithMatchCriteriaForLocusDqb1AndNoHlaDataAtDqb1_ShouldHaveValidationError()
        {
            var result = validator.Validate(new Client.Models.SearchRequests.SearchRequest
            {
                SearchType = DonorType.Adult,
                MatchCriteria = new MismatchCriteria
                {
                    LocusMismatchA = new LocusMismatchCriteria(),
                    LocusMismatchB = new LocusMismatchCriteria(),
                    LocusMismatchDrb1 = new LocusMismatchCriteria(),
                    LocusMismatchDqb1 = new LocusMismatchCriteria(),
                },
                SearchHlaData = new SearchHlaData
                {
                    LocusSearchHlaA = new LocusSearchHla {SearchHla1 = "hla", SearchHla2 = "hla"},
                    LocusSearchHlaB = new LocusSearchHla {SearchHla1 = "hla", SearchHla2 = "hla"},
                    LocusSearchHlaDrb1 = new LocusSearchHla {SearchHla1 = "hla", SearchHla2 = "hla"},
                }
            });
            result.IsValid.Should().BeFalse();
        }
    }
}