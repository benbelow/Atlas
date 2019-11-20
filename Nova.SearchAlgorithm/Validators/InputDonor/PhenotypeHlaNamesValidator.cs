using FluentValidation;
using Nova.Utils.PhenotypeInfo;

namespace Nova.SearchAlgorithm.Validators.InputDonor
{
    public class PhenotypeHlaNamesValidator : AbstractValidator<PhenotypeInfo<string>>
    {
        public PhenotypeHlaNamesValidator()
        {
            RuleFor(x => x.A).NotNull().SetValidator(new RequiredLocusHlaNamesValidator());
            RuleFor(x => x.B).NotNull().SetValidator(new RequiredLocusHlaNamesValidator());
            RuleFor(x => x.Drb1).NotNull().SetValidator(new RequiredLocusHlaNamesValidator());

            RuleFor(x => x.C).SetValidator(new OptionalLocusHlaNamesValidator());
            RuleFor(x => x.Dqb1).SetValidator(new OptionalLocusHlaNamesValidator());
            RuleFor(x => x.Dpb1).SetValidator(new OptionalLocusHlaNamesValidator());
        }
    }
}