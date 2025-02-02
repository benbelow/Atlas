using Atlas.Common.Validation;
using Atlas.MatchingAlgorithm.Client.Models.Donors;
using Atlas.MatchingAlgorithm.Extensions;
using FluentValidation;

namespace Atlas.MatchingAlgorithm.Validators.DonorInfo
{
    public class SearchableDonorInformationValidator : AbstractValidator<SearchableDonorInformation>
    {
        public SearchableDonorInformationValidator()
        {
            RuleFor(x => x.DonorId).NotNull();
            RuleFor(x => x.HlaAsPhenotypeInfoTransfer()).SetValidator(new PhenotypeHlaNamesValidator());
        }
    }
}