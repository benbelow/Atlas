﻿using FluentValidation.TestHelper;
using Nova.SearchAlgorithm.Validators.InputDonor;
using Nova.Utils.PhenotypeInfo;
using NUnit.Framework;

namespace Nova.SearchAlgorithm.Test.Validators.InputDonor
{
    [TestFixture]
    public class InputDonorValidatorTests
    {
        private InputDonorValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new InputDonorValidator();
        }

        [Test]
        public void Validator_WhenHlaNamesMissing_ShouldHaveValidationError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.HlaNames, (PhenotypeInfo<string>) null);
        }
    }
}