using System;
using Atlas.Common.ApplicationInsights;
using Atlas.HlaMetadataDictionary.ExternalInterface.DependencyInjection;
using Atlas.HlaMetadataDictionary.ExternalInterface.Settings;
using Atlas.HlaMetadataDictionary.Repositories.MetadataRepositories;
using Atlas.HlaMetadataDictionary.Test.IntegrationTests.TestHelpers.FileBackedStorageStubs;
using Atlas.HlaMetadataDictionary.WmdaDataAccess;
using Atlas.MultipleAlleleCodeDictionary.ExternalInterface;
using Atlas.MultipleAlleleCodeDictionary.Settings;
using Atlas.MultipleAlleleCodeDictionary.Test.Integration.DependencyInjection;
using Atlas.MultipleAlleleCodeDictionary.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Atlas.HlaMetadataDictionary.Test.IntegrationTests.DependencyInjection
{
    public static class ServiceConfiguration
    {
        public static IServiceProvider CreateProvider()
        {
            var services = new ServiceCollection();
            services.RegisterFileBasedHlaMetadataDictionaryForTesting(sp => new ApplicationInsightsSettings {LogLevel = "Info"});
            return services.BuildServiceProvider();
        }

        public static void RegisterFileBasedHlaMetadataDictionaryForTesting(
            this IServiceCollection services,
            Func<IServiceProvider, ApplicationInsightsSettings> fetchApplicationInsightsSettings)
        {
            services.RegisterHlaMetadataDictionary(
                _ => new HlaMetadataDictionarySettings(),
                fetchApplicationInsightsSettings,
                _ => new MacDictionarySettings()
            );

            
            services.RegisterOptions<MacImportSettings>("MacImport");
            services.RegisterConfiguration();

            services.SetUpMacDictionaryWithFileBackedRepository(
                fetchApplicationInsightsSettings,
                DependencyInjectionUtils.OptionsReaderFor<MacImportSettings>());

            // Replace Repositories with File-Backed equivalents.
            RegisterConfiguration(services);
            services.AddScoped<IHlaScoringMetadataRepository, FileBackedHlaScoringMetadataRepository>();
            services.AddScoped<IHlaMatchingMetadataRepository, FileBackedHlaMatchingMetadataRepository>();
            services.AddScoped<IAlleleNamesMetadataRepository, FileBackedAlleleNamesMetadataRepository>();
            services.AddScoped<IDpb1TceGroupsMetadataRepository, FileBackedTceMetadataRepository>();
            services.AddScoped<IAlleleGroupsMetadataRepository, FileBackedAlleleGroupsMetadataRepository>();

            services.AddScoped(sp =>
            {
                var wmdaHlaNomenclatureVersionAccessor = Substitute.For<IWmdaHlaNomenclatureVersionAccessor>();
                wmdaHlaNomenclatureVersionAccessor.GetLatestStableHlaNomenclatureVersion().Returns(Constants.SnapshotHlaNomenclatureVersion);
                return wmdaHlaNomenclatureVersionAccessor;
            });
        }
        
        private static void RegisterConfiguration(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();

            services.AddSingleton<IConfiguration>(sp => configuration);
        }
    }
}