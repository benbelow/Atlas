﻿using FluentValidation;
using System.Collections.Generic;

namespace Nova.SearchAlgorithm.Client.Models
{
    public enum DonorType
    {
        // Do not renumber, these values are stored in the database as integers.
        Adult = 1,
        Cord = 2
    }

    public enum RegistryCode
    {
        // Do not renumber, these values are stored in the database as integers.
        AN = 1, // Anthony Nolan
        NHSBT = 2, // NHS Blood Transfusion
        WBS = 3, // Welsh Blood Service
        DKMS = 4, // German Marrow Donor Program,
        FRANCE = 5,
        NMDP = 6
    }

    [FluentValidation.Attributes.Validator(typeof(SearchRequestValidator))]
    public class SearchRequest
    {
        public DonorType SearchType { get; set; }
        public MismatchCriteria MatchCriteria { get; set; }
        public IEnumerable<RegistryCode> RegistriesToSearch { get; set; }
    }

    public class SearchRequestValidator : AbstractValidator<SearchRequest>
    {
        public SearchRequestValidator()
        {
            RuleFor(x => x.SearchType).NotEmpty();
            RuleFor(x => x.MatchCriteria).NotEmpty();
            RuleFor(x => x.RegistriesToSearch).NotEmpty();
        }
    }
}
