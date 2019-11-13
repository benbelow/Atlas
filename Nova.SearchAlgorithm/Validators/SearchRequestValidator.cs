using FluentValidation;
using Nova.SearchAlgorithm.Client.Models.SearchRequests;
using Nova.SearchAlgorithm.Helpers;
using System.Linq;

namespace Nova.SearchAlgorithm.Validators
{
    public class SearchRequestValidator : AbstractValidator<SearchRequest>
    {
        public SearchRequestValidator()
        {
            RuleFor(x => x.SearchType).NotNull().IsInEnum();
            RuleFor(x => x.MatchCriteria).NotNull().SetValidator(new MismatchCriteriaValidator());
            RuleFor(x => x.SearchHlaData).NotNull().SetValidator(new SearchHlaDataValidator());
            RuleFor(x => x.SearchHlaData.LocusSearchHlaC).NotNull().When(x => x.MatchCriteria?.LocusMismatchC != null);
            RuleFor(x => x.SearchHlaData.LocusSearchHlaDqb1).NotNull().When(x => x.MatchCriteria?.LocusMismatchDqb1 != null);
            RuleFor(x => x.RegistriesToSearch).NotEmpty();
            RuleForEach(x => x.RegistriesToSearch).IsInEnum();
            RuleFor(x => x.LociToExcludeFromAggregateScore)
                .NotNull()
                .Must(loci => loci?.All(l => l.ToAlgorithmLocus() != null) ?? false)
                .WithMessage("Only valid search loci can be excluded from aggregation. They are: A, B, C, Dpb1, Dqb1, Drb1");
        }
    }
}