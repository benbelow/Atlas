using System;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.MatchingAlgorithm.Common.Config;
using Atlas.MatchingAlgorithm.Common.Models.Matching;
using Atlas.MatchingAlgorithm.Data.Helpers;
using Atlas.MatchingAlgorithm.Data.Models;
using Atlas.MatchingAlgorithm.Data.Models.DonorInfo;
using Atlas.MatchingAlgorithm.Data.Models.Entities;
using Atlas.MatchingAlgorithm.Data.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlas.Common.Utils.Extensions;
using MoreLinq.Extensions;

namespace Atlas.MatchingAlgorithm.Data.Repositories.DonorRetrieval
{
    /// <summary>
    /// Provides methods for retrieving donor data used when returning search results. Does not perform any filtering of donors
    /// </summary>
    public interface IDonorInspectionRepository
    {
        Task<DonorInfo> GetDonor(int donorId);
        Task<Dictionary<int, DonorInfo>> GetDonors(IEnumerable<int> donorIds);
        Task<IEnumerable<DonorIdWithPGroupNames>> GetPGroupsForDonors(IEnumerable<int> donorIds);
    }

    public class DonorInspectionRepository : Repository, IDonorInspectionRepository
    {
        public DonorInspectionRepository(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider)
        {
        }

        public async Task<DonorInfo> GetDonor(int donorId)
        {
            using (var conn = new SqlConnection(ConnectionStringProvider.GetConnectionString()))
            {
                var donor = await conn.QuerySingleOrDefaultAsync<Donor>($"SELECT * FROM Donors WHERE DonorId = {donorId}", commandTimeout: 300);
                return donor?.ToDonorInfo();
            }
        }

        /// <summary>
        /// Fetches all PGroups for a batch of donors from the MatchingHlaAt$Locus tables
        /// </summary>
        public async Task<IEnumerable<DonorIdWithPGroupNames>> GetPGroupsForDonors(IEnumerable<int> donorIds)
        {
            donorIds = donorIds.ToList();
            if (!donorIds.Any())
            {
                return new List<DonorIdWithPGroupNames>();
            }

            var results = donorIds
                .Select(id => new DonorIdWithPGroupNames { DonorId = id, PGroupNames = new PhenotypeInfo<IEnumerable<string>>() })
                .ToList();
            using (var conn = new SqlConnection(ConnectionStringProvider.GetConnectionString()))
            {
                // TODO NOVA-1427: Do not fetch PGroups for loci that have already been matched at the DB level
                foreach (var locus in LocusSettings.MatchingOnlyLoci)
                {
                    var sql = $@"
SELECT m.DonorId, m.TypePosition, p.Name as PGroupName FROM {MatchingTableNameHelper.MatchingTableName(locus)} m
JOIN PGroupNames p 
ON m.PGroup_Id = p.Id
INNER JOIN (
    SELECT '{donorIds.FirstOrDefault()}' AS Id
    UNION ALL SELECT '{string.Join("' UNION ALL SELECT '", donorIds.Skip(1))}'
)
AS DonorIds 
ON m.DonorId = DonorIds.Id
";
                    var pGroups = await conn.QueryAsync<DonorMatchWithName>(sql, commandTimeout: 300);
                    foreach (var donorGroups in pGroups.GroupBy(p => p.DonorId))
                    {
                        foreach (var pGroupGroup in donorGroups.GroupBy(p => (TypePosition)p.TypePosition))
                        {
                            var donorResult = results.Single(r => r.DonorId == donorGroups.Key);
                            donorResult.PGroupNames = donorResult.PGroupNames.SetPosition(
                                locus,
                                pGroupGroup.Key.ToLocusPosition(),
                                pGroupGroup.Select(p => p.PGroupName)
                            );
                        }
                    }
                }
            }

            return results;
        }

        public async Task<Dictionary<int, DonorInfo>> GetDonors(IEnumerable<int> donorIds)
        {
            const int donorIdBatchSize = 50000;

            donorIds = donorIds.ToList();
            if (!donorIds.Any())
            {
                return new Dictionary<int, DonorInfo>();
            }

            var results = new List<DonorInfo>();

            // Batching is being used here, as SQL server's query processor is limited in the number of donor IDs it can handle in a single query.
            foreach (var donorIdBatch in donorIds.Batch(donorIdBatchSize))
            {
                results.AddRange(await GetDonorInfos(donorIds));
            }

            return results.ToDictionary(d => d.DonorId, d => d);
        }

        private async Task<IEnumerable<DonorInfo>> GetDonorInfos(IEnumerable<int> donorIds)
        {
            await using (var conn = new SqlConnection(ConnectionStringProvider.GetConnectionString()))
            {
                var donorIdsString = donorIds.Select(id => id.ToString()).StringJoin(",");

                // Note that a previous iteration of this code used a JOIN on UNIONs of hard-coded Ids.
                // A thorough perf test demonstrated that this 'IN' produces an identical QueryPlan, and compiles faster.
                var sql = $@"SELECT * FROM Donors WHERE DonorId IN({donorIdsString})";

                var donors = await conn.QueryAsync<Donor>(sql, commandTimeout: 1200);
                return donors.Select(d => d.ToDonorInfo());
            }
        }
    }
}