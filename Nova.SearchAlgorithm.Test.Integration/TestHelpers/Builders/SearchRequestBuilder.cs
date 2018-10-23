using System;
using System.Collections.Generic;
using System.Linq;
using Nova.SearchAlgorithm.Client.Models;
using Nova.SearchAlgorithm.Client.Models.SearchRequests;
using Nova.SearchAlgorithm.Common.Models;

namespace Nova.SearchAlgorithm.Test.Integration.TestHelpers.Builders
{
    public class SearchRequestBuilder
    {
        private readonly SearchRequest searchRequest;

        public SearchRequestBuilder()
        {
            searchRequest = new SearchRequest()
            {
                SearchType = DonorType.Adult,
                RegistriesToSearch = new List<RegistryCode> {RegistryCode.AN},
                MatchCriteria = new MismatchCriteria
                {
                    LocusMismatchA = new LocusMismatchCriteria(),
                    LocusMismatchB = new LocusMismatchCriteria(),
                    LocusMismatchDrb1 = new LocusMismatchCriteria()
                },
                SearchHlaData = new SearchHlaData
                {
                    LocusSearchHlaA = new LocusSearchHla(),
                    LocusSearchHlaB = new LocusSearchHla(),
                    LocusSearchHlaC = new LocusSearchHla(),
                    LocusSearchHlaDqb1 = new LocusSearchHla(),
                    LocusSearchHlaDrb1 = new LocusSearchHla(),
                }
            };
        }

        public SearchRequestBuilder WithTotalMismatchCount(int mismatchCount)
        {
            searchRequest.MatchCriteria.DonorMismatchCount = mismatchCount;
            return this;
        }

        public SearchRequestBuilder WithLocusMatchCriteria(Locus locus, LocusMismatchCriteria locusMatchCriteria)
        {
            switch (locus)
            {
                case Locus.A:
                    searchRequest.MatchCriteria.LocusMismatchA = locusMatchCriteria;
                    break;
                case Locus.B:
                    searchRequest.MatchCriteria.LocusMismatchB = locusMatchCriteria;
                    break;
                case Locus.C:
                    searchRequest.MatchCriteria.LocusMismatchC = locusMatchCriteria;
                    break;
                case Locus.Dpb1:
                    throw new NotImplementedException();
                case Locus.Dqb1:
                    searchRequest.MatchCriteria.LocusMismatchDqb1 = locusMatchCriteria;
                    break;
                case Locus.Drb1:
                    searchRequest.MatchCriteria.LocusMismatchDrb1 = locusMatchCriteria;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(locus), locus, null);
            }

            return this;
        }

        public SearchRequestBuilder WithLocusMismatchCount(Locus locus, int locusMismatchCount)
        {
            switch (locus)
            {
                case Locus.A:
                    searchRequest.MatchCriteria.LocusMismatchA.MismatchCount = locusMismatchCount;
                    break;
                case Locus.B:
                    searchRequest.MatchCriteria.LocusMismatchB.MismatchCount = locusMismatchCount;
                    break;
                case Locus.C:
                    if (searchRequest.MatchCriteria.LocusMismatchC == null)
                    {
                        searchRequest.MatchCriteria.LocusMismatchC = new LocusMismatchCriteria();
                    }

                    searchRequest.MatchCriteria.LocusMismatchC.MismatchCount = locusMismatchCount;
                    break;
                case Locus.Dpb1:
                    throw new NotImplementedException();
                case Locus.Dqb1:
                    if (searchRequest.MatchCriteria.LocusMismatchDqb1 == null)
                    {
                        searchRequest.MatchCriteria.LocusMismatchDqb1 = new LocusMismatchCriteria();
                    }

                    searchRequest.MatchCriteria.LocusMismatchDqb1.MismatchCount = locusMismatchCount;
                    break;
                case Locus.Drb1:
                    searchRequest.MatchCriteria.LocusMismatchDrb1.MismatchCount = locusMismatchCount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(locus), locus, null);
            }

            return this;
        }

        public SearchRequestBuilder WithMismatchCountAtLoci(IEnumerable<Locus> loci, int locusMismatchCount)
        {
            return loci.Aggregate(this, (current, locus) => current.WithLocusMismatchCount(locus, locusMismatchCount));
        }

        public SearchRequestBuilder WithLocusMatchHla(Locus locus, TypePosition position, string hlaString)
        {
            switch (locus)
            {
                case Locus.A:
                    switch (position)
                    {
                        case TypePosition.One:
                            searchRequest.SearchHlaData.LocusSearchHlaA.SearchHla1 = hlaString;
                            break;
                        case TypePosition.Two:
                            searchRequest.SearchHlaData.LocusSearchHlaA.SearchHla2 = hlaString;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(position), position, null);
                    }

                    break;
                case Locus.B:
                    switch (position)
                    {
                        case TypePosition.One:
                            searchRequest.SearchHlaData.LocusSearchHlaB.SearchHla1 = hlaString;
                            break;
                        case TypePosition.Two:
                            searchRequest.SearchHlaData.LocusSearchHlaB.SearchHla2 = hlaString;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(position), position, null);
                    }

                    break;
                case Locus.C:
                    switch (position)
                    {
                        case TypePosition.One:
                            searchRequest.SearchHlaData.LocusSearchHlaC.SearchHla1 = hlaString;
                            break;
                        case TypePosition.Two:
                            searchRequest.SearchHlaData.LocusSearchHlaC.SearchHla2 = hlaString;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(position), position, null);
                    }

                    break;
                case Locus.Dpb1:
                    throw new NotImplementedException();
                case Locus.Dqb1:
                    switch (position)
                    {
                        case TypePosition.One:
                            searchRequest.SearchHlaData.LocusSearchHlaDqb1.SearchHla1 = hlaString;
                            break;
                        case TypePosition.Two:
                            searchRequest.SearchHlaData.LocusSearchHlaDqb1.SearchHla2 = hlaString;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(position), position, null);
                    }

                    break;
                case Locus.Drb1:
                    switch (position)
                    {
                        case TypePosition.One:
                            searchRequest.SearchHlaData.LocusSearchHlaDrb1.SearchHla1 = hlaString;
                            break;
                        case TypePosition.Two:
                            searchRequest.SearchHlaData.LocusSearchHlaDrb1.SearchHla2 = hlaString;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(position), position, null);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(locus), locus, null);
            }

            return this;
        }

        public SearchRequestBuilder WithSearchType(DonorType donorType)
        {
            searchRequest.SearchType = donorType;
            return this;
        }

        public SearchRequestBuilder ForRegistries(IEnumerable<RegistryCode> registryCodes)
        {
            searchRequest.RegistriesToSearch = registryCodes;
            return this;
        }

        public SearchRequestBuilder ForAdditionalRegistry(RegistryCode registryCode)
        {
            searchRequest.RegistriesToSearch = searchRequest.RegistriesToSearch.ToList().Concat(new[] {registryCode}).Distinct();
            return this;
        }

        public SearchRequestBuilder WithSearchHla(PhenotypeInfo<string> searchHla)
        {
            return WithLocusMatchHla(Locus.A, TypePosition.One, searchHla.A_1)
                .WithLocusMatchHla(Locus.A, TypePosition.Two, searchHla.A_2)
                .WithLocusMatchHla(Locus.B, TypePosition.One, searchHla.B_1)
                .WithLocusMatchHla(Locus.B, TypePosition.Two, searchHla.B_2)
                .WithLocusMatchHla(Locus.Drb1, TypePosition.One, searchHla.DRB1_1)
                .WithLocusMatchHla(Locus.Drb1, TypePosition.Two, searchHla.DRB1_2)
                .WithLocusMatchHla(Locus.C, TypePosition.One, searchHla.C_1)
                .WithLocusMatchHla(Locus.C, TypePosition.Two, searchHla.C_2)
                .WithLocusMatchHla(Locus.Dqb1, TypePosition.One, searchHla.DQB1_1)
                .WithLocusMatchHla(Locus.Dqb1, TypePosition.Two, searchHla.DQB1_2);
        }

        public SearchRequest Build()
        {
            return searchRequest;
        }
    }
}