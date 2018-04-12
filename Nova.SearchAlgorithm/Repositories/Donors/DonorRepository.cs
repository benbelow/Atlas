﻿using AutoMapper;
using Microsoft.WindowsAzure.Storage.Table;
using Nova.SearchAlgorithm.Models;
using Nova.SearchAlgorithm.Client.Models;
using Nova.SearchAlgorithm.Repositories.Donors.AzureStorage;
using System.Collections.Generic;
using System.Linq;


namespace Nova.SearchAlgorithm.Repositories.Donors
{
    public interface IDonorRepository
    {
        SearchableDonor GetDonor(int donorId);
        IEnumerable<HlaMatch> GetMatchesForDonor(int donorId);
        IEnumerable<SearchableDonor> AllDonors();
        IEnumerable<HlaMatch> GetDonorMatchesAtLocus(SearchType searchType, IEnumerable<RegistryCode> registries, string locus, LocusSearchCriteria criteria);
        void InsertDonor(SearchableDonor donor);
        void UpdateDonorWithNewHla(SearchableDonor donor);
    }

    static class TableQueryExtensions
    {
        public static TableQuery<TElement> AndWhere<TElement>(this TableQuery<TElement> @this, string filter)
        {
            @this.FilterString = TableQuery.CombineFilters(@this.FilterString, TableOperators.And, filter);
            return @this;
        }

        public static TableQuery<TElement> OrWhere<TElement>(this TableQuery<TElement> @this, string filter)
        {
            @this.FilterString = @this.FilterString == null ? filter : TableQuery.CombineFilters(@this.FilterString, TableOperators.Or, filter);
            return @this;
        }

        public static TableQuery<TElement> NotWhere<TElement>(this TableQuery<TElement> @this, string filter)
        {
            @this.FilterString = TableQuery.CombineFilters(@this.FilterString, TableOperators.Not, filter);
            return @this;
        }
    }

    public class DonorRepository : IDonorRepository
    {
        private const string DonorTableReference = "Donors";
        private const string MatchTableReference = "Matches";
        private readonly CloudTable donorTable;
        private readonly CloudTable matchTable;
        private readonly IMapper mapper;

        public DonorRepository(IMapper mapper, ICloudTableFactory cloudTableFactory)
        {
            donorTable = cloudTableFactory.GetTable(DonorTableReference);
            matchTable = cloudTableFactory.GetTable(MatchTableReference);
            this.mapper = mapper;
        }
        
        public IEnumerable<HlaMatch> GetDonorMatchesAtLocus(SearchType searchType, IEnumerable<RegistryCode> registries, string locus, LocusSearchCriteria criteria)
        {
            var matchesFromPositionOne = GetMatches(locus, criteria.HlaNamesToMatchInPositionOne);
            var matchesFromPositionTwo = GetMatches(locus, criteria.HlaNamesToMatchInPositionTwo);

            return matchesFromPositionOne.Select(m => m.ToHlaMatch(TypePositions.One)).Union(matchesFromPositionTwo.Select(m => m.ToHlaMatch(TypePositions.Two)));
        }

        private IEnumerable<HlaMatchTableEntity> GetMatches(string locus, IEnumerable<string> namesToMatch)
        {
            var matchesQuery = new TableQuery<HlaMatchTableEntity>();
            foreach (string name in namesToMatch)
            {
                matchesQuery = matchesQuery.OrWhere(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, HlaMatchTableEntity.GeneratePartitionKey(locus, name)));
            }

            return matchTable.ExecuteQuery(matchesQuery);
        }

        public SearchableDonor GetDonor(int donorId)
        {
            var donorQuery = new TableQuery<DonorTableEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, donorId.ToString()));
            return donorTable.ExecuteQuery(donorQuery).Select(dte => dte.ToSearchableDonor(mapper)).FirstOrDefault();
        }

        public IEnumerable<HlaMatch> GetMatchesForDonor(int donorId)
        {
            return AllMatchesForDonor(donorId).Select(m => m.ToHlaMatch(0));
        }

        public void InsertDonor(SearchableDonor donor)
        {
            var insertDonor = TableOperation.InsertOrReplace(donor.ToTableEntity(mapper));
            donorTable.Execute(insertDonor);

            UpdateDonorHlaMatches(donor); donor.MatchingHla.Each((locusName, position, matchingHla) => InsertLocusMatch(locusName, position, matchingHla, donor.DonorId));

            // TODO:NOVA-929 if this method stays, sort out a return value
        }

        // TODO:NOVA-929 This will be too many donors
        // Can we stream them in batches with IEnumerable?
        public IEnumerable<SearchableDonor> AllDonors()
        {
            var query = new TableQuery<DonorTableEntity>();
            return donorTable.ExecuteQuery(query).Select(dte => dte.ToSearchableDonor(mapper));
        }

        public void UpdateDonorWithNewHla(SearchableDonor donor)
        {
            // Update the donor itself
            var insertDonor = TableOperation.InsertOrReplace(donor.ToTableEntity(mapper));
            donorTable.Execute(insertDonor);
            UpdateDonorHlaMatches(donor);
        }

        private void UpdateDonorHlaMatches(SearchableDonor donor)
        {
            // First delete all the old matches
            var matches = AllMatchesForDonor(donor.DonorId);
            foreach (var match in matches)
            {
                matchTable.Execute(TableOperation.Delete(match));
            }

            // Add back the new matches
            donor.MatchingHla.Each((locusName, position, matchingHla) => InsertLocusMatch(locusName, position, matchingHla, donor.DonorId));
        }

        private IEnumerable<HlaMatchTableEntity> AllMatchesForDonor(int donorId)
        { 
            var matchesQuery = new TableQuery<HlaMatchTableEntity>().Where(TableQuery.GenerateFilterConditionForInt("DonorId", QueryComparisons.Equal, donorId));

            //var matchesQuery = new TableQuery<HlaMatchTableEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, donorId.ToString()));

            return matchTable.ExecuteQuery(matchesQuery);
        }

        private void InsertLocusMatch(string locusName, TypePositions typePosition, MatchingHla matchingHla, int donorId)
        {
            if (matchingHla == null)
            {
                return;
            }

            foreach (string matchName in (matchingHla.MatchingProteinGroups ?? new List<string>()).Union(matchingHla.MatchingSerologyNames))
            {
                var insertMatch = TableOperation.InsertOrMerge(new HlaMatchTableEntity(locusName, typePosition, matchName, donorId));
                matchTable.Execute(insertMatch);
            }
        }
    }
}
