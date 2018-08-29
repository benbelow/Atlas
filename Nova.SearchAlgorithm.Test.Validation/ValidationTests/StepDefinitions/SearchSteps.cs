﻿using FluentAssertions;
using Nova.SearchAlgorithm.Client.Models;
using Nova.SearchAlgorithm.Client.Models.SearchResults;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Test.Integration.TestHelpers.Builders;
using Nova.SearchAlgorithm.Test.Validation.TestData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nova.SearchAlgorithm.Test.Validation.TestData.Models;
using Nova.SearchAlgorithm.Test.Validation.TestData.Services.PatientDataSelection;
using Nova.Utils.Models;
using TechTalk.SpecFlow;
using Locus = Nova.SearchAlgorithm.Common.Models.Locus;

namespace Nova.SearchAlgorithm.Test.Validation.ValidationTests.StepDefinitions
{
    [Binding]
    public class SearchSteps
    {
        [Given(@"the search type is (.*)")]
        public void GivenTheSearchTypeIs(string searchType)
        {
            var donorType = (DonorType) Enum.Parse(typeof(DonorType), searchType, true);
            var searchRequest = ScenarioContext.Current.Get<SearchRequestBuilder>();
            ScenarioContext.Current.Set(searchRequest.WithSearchType(donorType));
        }

        [Given(@"the search is run against the Anthony Nolan registry only")]
        public void GivenTheSearchIsRunForAnthonyNolansRegistryOnly()
        {
            var searchRequest = ScenarioContext.Current.Get<SearchRequestBuilder>();
            ScenarioContext.Current.Set(searchRequest.ForRegistries(new []{ RegistryCode.AN }));
        }

        [Given(@"the search is run for aligned registries")]
        public void GivenTheSearchIsRunForAlignedRegistries()
        {
            var searchRequest = ScenarioContext.Current.Get<SearchRequestBuilder>();
            ScenarioContext.Current.Set(searchRequest.ForRegistries(new []
            {
                RegistryCode.AN,
                RegistryCode.NHSBT,
                RegistryCode.WBS,
                RegistryCode.DKMS
            }));
        }

        [Given(@"the search is run for the registry: (.*)")]
        public void GivenTheSearchIsRunForRegistry(string registryString)
        {
            // If the search team prefer to write the tests with expanded registry names, we will need to manually map to the enum
            var registry = (RegistryCode) Enum.Parse(typeof(RegistryCode), registryString, true);
            var searchRequest = ScenarioContext.Current.Get<SearchRequestBuilder>();
            ScenarioContext.Current.Set(searchRequest.ForAdditionalRegistry(registry));
        }

        [When(@"I run a 6/6 search")]
        public async Task WhenIRunASixOutOfSixSearch()
        {
            var patientDataSelector = ScenarioContext.Current.Get<IPatientDataFactory>();
            var searchRequestBuilder = ScenarioContext.Current.Get<SearchRequestBuilder>();
            var searchHla = patientDataSelector.GetPatientHla();

            var searchRequest = searchRequestBuilder
                .WithTotalMismatchCount(0)
                .WithLocusMismatchCount(Locus.A, 0)
                .WithLocusMismatchCount(Locus.B, 0)
                .WithLocusMismatchCount(Locus.Drb1, 0)
                .WithSearchHla(searchHla)
                .Build();

            ScenarioContext.Current.Set(await AlgorithmTestingService.Search(searchRequest));
        }
        
        [When(@"I run a 8/8 search")]
        [When(@"I run an 8/8 search")]
        public async Task WhenIRunAnEightOutOfEightSearch()
        {
            var patientDataSelector = ScenarioContext.Current.Get<IPatientDataFactory>();
            var searchRequestBuilder = ScenarioContext.Current.Get<SearchRequestBuilder>();
            var searchHla = patientDataSelector.GetPatientHla();

            var searchRequest = searchRequestBuilder
                .WithTotalMismatchCount(0)
                .WithLocusMismatchCount(Locus.A, 0)
                .WithLocusMismatchCount(Locus.B, 0)
                .WithLocusMismatchCount(Locus.Drb1, 0)
                .WithLocusMismatchCount(Locus.C, 0)
                .WithSearchHla(searchHla)
                .Build();

            ScenarioContext.Current.Set(await AlgorithmTestingService.Search(searchRequest));
        }
               
        [When(@"I run a 4/8 search")]
        public async Task WhenIRunAFourOutOfEightSearch()
        {
            var patientDataSelector = ScenarioContext.Current.Get<IPatientDataFactory>();
            var searchRequestBuilder = ScenarioContext.Current.Get<SearchRequestBuilder>();
            var searchHla = patientDataSelector.GetPatientHla();

            var searchRequest = searchRequestBuilder
                .WithTotalMismatchCount(4)
                .WithLocusMismatchCount(Locus.A, 2)
                .WithLocusMismatchCount(Locus.B, 2)
                .WithLocusMismatchCount(Locus.Drb1, 2)
                .WithLocusMismatchCount(Locus.C, 2)
                .WithSearchHla(searchHla)
                .Build();

            ScenarioContext.Current.Set(await AlgorithmTestingService.Search(searchRequest));
        }

        [When(@"I run a 10/10 search for each patient")]
        public async Task WhenIRunATenOutOfTenSearchForEachPatient()
        {
            var selector = ScenarioContext.Current.Get<IMultiplePatientDataFactory>();

            var patientResults = new List<PatientResultSet>();
            
            foreach (var patientDataSelector in selector.PatientDataFactories)
            {
                var searchHla = patientDataSelector.GetPatientHla();
                var searchRequestBuilder = ScenarioContext.Current.Get<SearchRequestBuilder>();

                var searchRequest = searchRequestBuilder
                    .WithTotalMismatchCount(0)
                    .WithLocusMismatchCount(Locus.A, 0)
                    .WithLocusMismatchCount(Locus.B, 0)
                    .WithLocusMismatchCount(Locus.Drb1, 0)
                    .WithLocusMismatchCount(Locus.C, 0)
                    .WithLocusMismatchCount(Locus.Dqb1, 0)
                    .WithSearchHla(searchHla)
                    .Build();

                var results = await AlgorithmTestingService.Search(searchRequest);
                patientResults.Add(new PatientResultSet
                {
                    PatientDataFactory = patientDataSelector,
                    SearchResultSet = results,
                });
            }

            ScenarioContext.Current.Set(patientResults);
        }

        [When(@"I run a 10/10 search")]
        public async Task WhenIRunATenOutOfTenSearch()
        {
            var patientDataSelector = ScenarioContext.Current.Get<IPatientDataFactory>();
            var searchRequestBuilder = ScenarioContext.Current.Get<SearchRequestBuilder>();
            var searchHla = patientDataSelector.GetPatientHla();

            var searchRequest = searchRequestBuilder
                .WithTotalMismatchCount(0)
                .WithLocusMismatchCount(Locus.A, 0)
                .WithLocusMismatchCount(Locus.B, 0)
                .WithLocusMismatchCount(Locus.Drb1, 0)
                .WithLocusMismatchCount(Locus.C, 0)
                .WithLocusMismatchCount(Locus.Dqb1, 0)
                .WithSearchHla(searchHla)
                .Build();

            ScenarioContext.Current.Set(await AlgorithmTestingService.Search(searchRequest));
        }

        [When(@"I run a 9/10 search at locus (.*)")]
        public async Task WhenIRunANineOutOfTenSearchAtLocus(string locusString)
        {
            var patientDataSelector = ScenarioContext.Current.Get<IPatientDataFactory>();
            var searchRequestBuilder = ScenarioContext.Current.Get<SearchRequestBuilder>();

            var searchHla = patientDataSelector.GetPatientHla();
            var locus = (Locus) Enum.Parse(typeof(Locus), locusString, true);
            var fullyMatchedLoci = LocusHelpers.AllLoci().Except(new[] {Locus.Dpb1, locus});

            var searchRequest = searchRequestBuilder
                .WithTotalMismatchCount(1)
                .WithLocusMismatchCount(locus, 1)
                .WithMismatchCountAtLoci(fullyMatchedLoci, 0)
                .WithSearchHla(searchHla)
                .Build();

            ScenarioContext.Current.Set(await AlgorithmTestingService.Search(searchRequest));
        }
        
        [When(@"I run a 8/10 search")]
        [When(@"I run an 8/10 search")]
        public async Task WhenIRunAnEightOutOfTenSearch()
        {
            var patientDataSelector = ScenarioContext.Current.Get<IPatientDataFactory>();
            var searchRequestBuilder = ScenarioContext.Current.Get<SearchRequestBuilder>();
            var searchHla = patientDataSelector.GetPatientHla();
            var allowedMismatchLoci = LocusHelpers.AllLoci().Except(new[] {Locus.Dpb1});

            var searchRequest = searchRequestBuilder
                .WithTotalMismatchCount(2)
                .WithMismatchCountAtLoci(allowedMismatchLoci, 2)
                .WithSearchHla(searchHla)
                .Build();

            ScenarioContext.Current.Set(await AlgorithmTestingService.Search(searchRequest));
        }

        [Then(@"The result should contain at least one donor")]
        public void ThenTheResultShouldContainAtLeastOneDonor()
        {
            var results = ScenarioContext.Current.Get<SearchResultSet>();
            results.SearchResults.Count().Should().BeGreaterThan(0);
        }
        
        [Then(@"the results should contain the specified donor")]
        public void ThenTheResultShouldContainTheSpecifiedDonor()
        {
            var patientDataSelector = ScenarioContext.Current.Get<IPatientDataFactory>();
            var results = ScenarioContext.Current.Get<SearchResultSet>();
            
            results.SearchResults.Should().Contain(r => r.DonorId == patientDataSelector.GetExpectedMatchingDonorIds().Single());
        } 
        
        [Then(@"the results should contain all specified donors")]
        public void ThenTheResultShouldContainAllSpecifiedDonors()
        {
            var patientDataSelector = ScenarioContext.Current.Get<IPatientDataFactory>();
            var results = ScenarioContext.Current.Get<SearchResultSet>();

            foreach (var donorId in patientDataSelector.GetExpectedMatchingDonorIds())
            {
                results.SearchResults.Should().Contain(r => r.DonorId == donorId);                
            }
        } 
        
        [Then(@"the results should not contain the specified donor")]
        public void ThenTheResultShouldNotContainTheSpecifiedDonor()
        {
            var patientDataSelector = ScenarioContext.Current.Get<IPatientDataFactory>();
            var results = ScenarioContext.Current.Get<SearchResultSet>();
            
            results.SearchResults.Should().NotContain(r => r.DonorId == patientDataSelector.GetExpectedMatchingDonorIds().Single());
        }
        
        [Then(@"each set of results should contain the specified donor")]
        public void ThenEachSetOfResultsShouldContainTheSpecifiedDonor()
        {
            var patientResultSets = ScenarioContext.Current.Get<List<PatientResultSet>>();

            foreach (var resultSet in patientResultSets)
            {
                var searchResultsForPatient = resultSet.SearchResultSet.SearchResults;
                var patientDataSelector = resultSet.PatientDataFactory;
                searchResultsForPatient.Should().Contain(r => r.DonorId == patientDataSelector.GetExpectedMatchingDonorIds().Single());
            }
        }
        
        [Then(@"The result should contain no donors")]
        public void ThenTheResultShouldContainNoDonors()
        {
            var results = ScenarioContext.Current.Get<SearchResultSet>();
            results.SearchResults.Count().Should().Be(0);
        }
    }
}