﻿using Autofac;
using Hangfire;
using Nova.SearchAlgorithm.MatchingDictionary.Repositories;
using Owin;
using System.Configuration;
using Nova.SearchAlgorithm.Services.DonorImport;

namespace Nova.SearchAlgorithm.Config
{
    public static class HangfireConfig
    {
        public static void ConfigureHangfire(this IAppBuilder app, IContainer container)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["HangfireSqlConnectionString"].ConnectionString);
            GlobalConfiguration.Configuration.UseAutofacActivator(container);

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            
            BackgroundJob.Enqueue<IAntigenCachingService>(antigenCachingService => antigenCachingService.GenerateAntigenCache());
            BackgroundJob.Enqueue<IHlaMatchingLookupRepository>(hlaMatchingLookupRepository => hlaMatchingLookupRepository.LoadDataIntoMemory(Configuration.HlaDatabaseVersion));
            BackgroundJob.Enqueue<IAlleleNamesLookupRepository>(alleleNamesLookupRepository => alleleNamesLookupRepository.LoadDataIntoMemory(Configuration.HlaDatabaseVersion));
            BackgroundJob.Enqueue<IHlaScoringLookupRepository>(hlaScoringLookupRepository => hlaScoringLookupRepository.LoadDataIntoMemory(Configuration.HlaDatabaseVersion));
            BackgroundJob.Enqueue<IDpb1TceGroupsLookupRepository>(dpb1TceGroupsLookupRepository => dpb1TceGroupsLookupRepository.LoadDataIntoMemory(Configuration.HlaDatabaseVersion));
        }
    }
}