﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Common.Repositories;
using Nova.SearchAlgorithm.Data.Models;
using Nova.SearchAlgorithm.Extensions;
using Nova.Utils.ApplicationInsights;

namespace Nova.SearchAlgorithm.Repositories.Donors.AzureStorage
{
    public class CloudTableStorage : IDonorDocumentStorage
    {
        private readonly ICloudTableFactory cloudTableFactory;
        private readonly ILogger logger;
        private readonly ITableReferenceRepository tableReferenceRepository;
        public const string DonorTableReference = "Donors";
        public const string MatchTableReference = "Matches";
        private readonly CloudTable donorTable;
        private CloudTable matchTable;

        public CloudTableStorage(ICloudTableFactory cloudTableFactory, ILogger logger, ITableReferenceRepository tableReferenceRepository)
        {
            this.cloudTableFactory = cloudTableFactory;
            this.logger = logger;
            this.tableReferenceRepository = tableReferenceRepository;
            donorTable = cloudTableFactory.GetTable(DonorTableReference);
            var currentTableReferenceTask = Task.Run(() => tableReferenceRepository.GetCurrentTableReference(MatchTableReference));
            currentTableReferenceTask.Wait();
            matchTable = cloudTableFactory.GetTable(currentTableReferenceTask.Result);
        }

        public Task<int> HighestDonorId()
        {
            return Task.FromResult(Enum.GetValues(typeof(RegistryCode)).Cast<RegistryCode>()
                .Select(rc =>
                {
                    TableQuery<DonorTableEntity> query = new TableQuery<DonorTableEntity>()
                        .Where(
                            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, rc.ToString()));

                    // Should be in order of row key ascending (within each partition)
                    return donorTable.ExecuteQuery(query).Reverse().Take(1).Select(d => d.DonorId).FirstOrDefault();
                })
                .Max());
        }

        public Task<IEnumerable<PotentialHlaMatchRelation>> GetDonorMatchesAtLocus(Locus locus,
            LocusSearchCriteria criteria)
        {
            var matchesFromPositionOne = GetMatches(locus, criteria.PGroupsToMatchInPositionOne);
            var matchesFromPositionTwo = GetMatches(locus, criteria.PGroupsToMatchInPositionTwo);

            return Task.FromResult(matchesFromPositionOne.Select(m => m.ToPotentialHlaMatchRelation(TypePositions.One))
                .Union(matchesFromPositionTwo.Select(m => m.ToPotentialHlaMatchRelation(TypePositions.Two))));
        }

        private IEnumerable<PotentialHlaMatchRelationTableEntity> GetMatches(Locus locus, IEnumerable<string> namesToMatch)
        {
            const int filterClauseLimit = 20;

            // Enumerate once
            var namesToMatchList = namesToMatch.ToList();

            if (!namesToMatchList.Any())
            {
                return Enumerable.Empty<PotentialHlaMatchRelationTableEntity>();
            }

            
            var results = namesToMatchList.Batch(filterClauseLimit).AsParallel().Select(batch =>
            {
                var matchesQuery = new TableQuery<PotentialHlaMatchRelationTableEntity>();
                foreach (string name in batch)
                {
                    matchesQuery = matchesQuery.OrWhere(TableQuery.GenerateFilterCondition("RowKey",
                        QueryComparisons.Equal,
                        PotentialHlaMatchRelationTableEntity.GenerateRowKey(locus, name)));
                }

                return matchTable.ExecuteQuery(matchesQuery);
            });

            // De-duplicate
            return results.SelectMany(x => x).Distinct();
        }

        public Task<DonorResult> GetDonor(int donorId)
        {
            var donorQuery = new TableQuery<DonorTableEntity>().Where(
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, donorId.ToString()));
            return Task.Run(() =>
                donorTable.ExecuteQuery(donorQuery).Select(dte => dte.ToDonorResult()).FirstOrDefault());
        }

        public Task InsertBatchOfDonors(IEnumerable<RawInputDonor> donors)
        {
            var allRegistryCodes = Enum.GetValues(typeof(RegistryCode)).Cast<RegistryCode>();
            return Task.WhenAll(allRegistryCodes
                .Where(rc => donors.Any(d => d.RegistryCode == rc))
                .Select(rc =>
                {
                    var batchOperation = new TableBatchOperation();
                    foreach (var donor in donors.Where(d => d.RegistryCode == rc))
                    {
                        batchOperation.Insert(donor.ToTableEntity());
                    }

                    return donorTable.ExecuteBatchAsync(batchOperation);
                }));
        }

        public void SetupForHlaRefresh()
        {
            var newTableReference = tableReferenceRepository.GetNewTableReference(MatchTableReference);
            matchTable = cloudTableFactory.GetTable(newTableReference);
        }

        public IBatchQueryAsync<DonorResult> AllDonors()
        {
            var query = new TableQuery<DonorTableEntity>();
            return new CloudTableDonorBatchQueryAsync(query, donorTable);
        }

        public async Task RefreshMatchingGroupsForExistingDonor(InputDonor donor)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            // Update the donor itself
            var insertDonor = TableOperation.InsertOrReplace(donor.ToTableEntity());
            await donorTable.ExecuteAsync(insertDonor);
            
            logger.SendTrace("Updated donor", LogLevel.Info, new Dictionary<string, string>
            {
                { "Time", stopwatch.ElapsedMilliseconds.ToString() },
                { "DonorId", donor.DonorId.ToString() }
            });

            await UpdateDonorHlaMatches(donor);
        }

        private async Task UpdateDonorHlaMatches(InputDonor donor)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Add back the new matches
            var newMatches = donor.MatchingHla
                .FlatMap((locusName, matchingHla1, matchingHla2) =>
                    ConvertToPotentialHlaMatchRelationTableEntities(locusName, matchingHla1, matchingHla2,
                        donor.DonorId))
                .SelectMany(x => x)
                .ToList();

            await ExecuteMatchOperationsInBatches(newMatches, (batchOp, match) => batchOp.Insert(match));
            
            logger.SendTrace("Updated new matches", LogLevel.Info, new Dictionary<string, string>
            {
                { "Time", stopwatch.ElapsedMilliseconds.ToString() },
                { "DonorId", donor.DonorId.ToString() },
                { "Matches", newMatches.Count().ToString() },
            });
        }

        private async Task ExecuteMatchOperationsInBatches(
            List<PotentialHlaMatchRelationTableEntity> matches,
            Action<TableBatchOperation, PotentialHlaMatchRelationTableEntity> addOperation)
        {
            foreach (var batch in matches.Batch(100))
            {
                var batchOperation = new TableBatchOperation();
                foreach (var match in batch)
                {
                    addOperation(batchOperation, match);
                }
                await matchTable.ExecuteBatchAsync(batchOperation);
            }
        }

        private IEnumerable<PotentialHlaMatchRelationTableEntity> AllMatchesForDonor(int donorId)
        {
            var matchesQuery = new TableQuery<PotentialHlaMatchRelationTableEntity>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
                    PotentialHlaMatchRelationTableEntity.GeneratePartitionKey(donorId)));

            return matchTable.ExecuteQuery(matchesQuery);
        }

        private IEnumerable<PotentialHlaMatchRelationTableEntity> ConvertToPotentialHlaMatchRelationTableEntities(
            Locus locusName, ExpandedHla matchingHla1, ExpandedHla matchingHla2, int donorId)
        {
            var list1 = (matchingHla1?.AllMatchingHlaNames() ?? Enumerable.Empty<string>()).ToList();
            var list2 = (matchingHla2?.AllMatchingHlaNames() ?? Enumerable.Empty<string>()).ToList();

            var combinedList = list1.Union(list2).ToList();

            if (!combinedList.Any())
            {
                return new List<PotentialHlaMatchRelationTableEntity>();
            }

            return combinedList.Select(matchName =>
            {
                var typePositions = (TypePositions.None);
                if (list1.Contains(matchName))
                {
                    typePositions |= TypePositions.One;
                }

                if (list2.Contains(matchName))
                {
                    typePositions |= TypePositions.Two;
                }

                return new PotentialHlaMatchRelationTableEntity(locusName, typePositions, matchName, donorId);
            });
        }
    }
}