﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlas.Common.ApplicationInsights;
using Atlas.MatchingAlgorithm.ApplicationInsights;
using Atlas.MatchingAlgorithm.Data.Models;
using Atlas.MatchingAlgorithm.Data.Models.Entities;
using Atlas.MatchingAlgorithm.Data.Repositories;
using Atlas.MatchingAlgorithm.Models;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders.TransientSqlDatabase.RepositoryFactories;
using Atlas.MatchingAlgorithm.Services.Donors;
using AutoMapper;

namespace Atlas.MatchingAlgorithm.Services.DonorManagement
{
    /// <summary>
    /// Manages the addition or removal of individual donors from the search algorithm database.
    /// </summary>
    public interface IDonorManagementService
    {
        Task ManageDonorBatchByAvailability(IEnumerable<DonorAvailabilityUpdate> donorAvailabilityUpdates);
    }

    public class DonorManagementService : IDonorManagementService
    {
        private class NonApplicableUpdate
        {
            public DonorManagementLog DonorManagementLog { get; }
            public DonorAvailabilityUpdate DonorAvailabilityUpdate { get; }

            public NonApplicableUpdate(DonorManagementLog log, DonorAvailabilityUpdate update)
            {
                DonorManagementLog = log;
                DonorAvailabilityUpdate = update;
            }
        }

        private const string TraceMessagePrefix = nameof(ManageDonorBatchByAvailability);

        private readonly IDonorManagementLogRepository logRepository;
        private readonly IDonorService donorService;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public DonorManagementService(
            IActiveRepositoryFactory repositoryFactory,
            IDonorService donorService,
            ILogger logger,
            IMapper mapper)
        {
            logRepository = repositoryFactory.GetDonorManagementLogRepository();
            this.donorService = donorService;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task ManageDonorBatchByAvailability(IEnumerable<DonorAvailabilityUpdate> donorAvailabilityUpdates)
        {
            var filteredUpdates = await FilterUpdates(donorAvailabilityUpdates);
            await ApplyDonorUpdates(filteredUpdates);
        }

        private async Task<IEnumerable<DonorAvailabilityUpdate>> FilterUpdates(IEnumerable<DonorAvailabilityUpdate> updates)
        {
            var latestUpdateInBatchPerDonorId = GetLatestUpdateInBatchPerDonorId(updates);
            return await GetApplicableUpdates(latestUpdateInBatchPerDonorId);
        }

        private static IEnumerable<DonorAvailabilityUpdate> GetLatestUpdateInBatchPerDonorId(IEnumerable<DonorAvailabilityUpdate> updates)
        {
            return updates
                .GroupBy(u => u.DonorId)
                .Select(grp => grp.OrderByDescending(u => u.UpdateDateTime).First());
        }

        /// <returns>Only returns those updates that are newer than the last update recorded in the
        /// donor management log, or those where the donor has no record of a previous update.</returns>
        private async Task<IEnumerable<DonorAvailabilityUpdate>> GetApplicableUpdates(IEnumerable<DonorAvailabilityUpdate> updates)
        {
            var allUpdates = updates.ToList();

            var existingLogs = await logRepository.GetDonorManagementLogBatch(allUpdates.Select(u => u.DonorId));

            // GroupJoin is equivalent to a LEFT OUTER JOIN
            var updatesWithLogs = allUpdates
                .GroupJoin(existingLogs,
                    update => update.DonorId,
                    log => log.DonorId,
                    (update, logs) => new { Update = update, Log = logs.SingleOrDefault() })
                .Select(a => new
                {
                    a.Update,
                    a.Log,
                    IsApplicable = a.Log == null || a.Update.UpdateDateTime > a.Log.LastUpdateDateTime
                })
                .ToList();

            var nonApplicableUpdates = updatesWithLogs
                .Where(u => !u.IsApplicable)
                .Select(u => new NonApplicableUpdate(u.Log, u.Update));
            LogNonApplicableUpdates(nonApplicableUpdates);

            return updatesWithLogs.Where(u => u.IsApplicable).Select(u => u.Update);
        }

        private void LogNonApplicableUpdates(IEnumerable<NonApplicableUpdate> nonApplicableUpdates)
        {
            nonApplicableUpdates = nonApplicableUpdates.ToList();

            if (!nonApplicableUpdates.Any())
            {
                return;
            }

            foreach (var update in nonApplicableUpdates)
            {
                logger.SendEvent(GetDonorUpdateNotAppliedEventModel(update));
            }

            logger.SendTrace(
                $"{TraceMessagePrefix}: {nonApplicableUpdates.Count()} donor updates were not applied " +
                "due to being older than previously applied updates (AI event logged for each update).",
                LogLevel.Warn);
        }

        private static DonorUpdateNotAppliedEventModel GetDonorUpdateNotAppliedEventModel(NonApplicableUpdate update)
        {
            return new DonorUpdateNotAppliedEventModel(update.DonorManagementLog.LastUpdateDateTime, update.DonorAvailabilityUpdate);
        }

        private async Task ApplyDonorUpdates(IEnumerable<DonorAvailabilityUpdate> updates)
        {
            var updatesList = updates.ToList();

            logger.SendTrace($"{TraceMessagePrefix}: {updatesList.Count} donor updates to be applied.", LogLevel.Info);

            // Note, the management log must be written to last to prevent the undesirable
            // scenario of the donor update failing after the log has been successfully updated.
            await AddOrUpdateDonors(updatesList);
            await SetDonorsAsUnavailableForSearch(updatesList);
            await CreateOrUpdateManagementLogBatch(updatesList);
        }

        private async Task AddOrUpdateDonors(IEnumerable<DonorAvailabilityUpdate> updates)
        {
            var availableDonors = updates
                .Where(update => update.IsAvailableForSearch && update.DonorInfo != null)
                .Select(d => d.DonorInfo)
                .ToList();

            if (availableDonors.Any())
            {
                logger.SendTrace($"{TraceMessagePrefix}: {availableDonors.Count} donors to be added or updated.", LogLevel.Info);

                await donorService.CreateOrUpdateDonorBatch(availableDonors);
            }
        }

        private async Task SetDonorsAsUnavailableForSearch(IEnumerable<DonorAvailabilityUpdate> updates)
        {
            var unavailableDonorIds = updates
                .Where(update => !update.IsAvailableForSearch)
                .Select(d => d.DonorId)
                .ToList();

            if (unavailableDonorIds.Any())
            {
                logger.SendTrace($"{TraceMessagePrefix}: {unavailableDonorIds.Count} donors to be marked as unavailable for search.", LogLevel.Info);

                await donorService.SetDonorBatchAsUnavailableForSearch(unavailableDonorIds);
            }
        }

        private async Task CreateOrUpdateManagementLogBatch(IEnumerable<DonorAvailabilityUpdate> appliedUpdates)
        {
            if (!appliedUpdates.Any())
            {
                return;
            }

            var infos = mapper.Map<IEnumerable<DonorManagementInfo>>(appliedUpdates);
            await logRepository.CreateOrUpdateDonorManagementLogBatch(infos);
        }
    }
}
