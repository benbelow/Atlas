﻿using System.Reflection;
using Atlas.Common.ApplicationInsights;
using Atlas.Common.Notifications;
using Atlas.HlaMetadataDictionary.ExternalInterface.Settings;
using Atlas.MatchingAlgorithm.DependencyInjection;
using Atlas.MatchingAlgorithm.Settings;
using Atlas.MatchingAlgorithm.Settings.Azure;
using Atlas.MatchingAlgorithm.Settings.ServiceBus;
using Atlas.MultipleAlleleCodeDictionary.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Atlas.Common.Utils.Extensions.DependencyInjectionUtils;

namespace Atlas.MatchingAlgorithm.Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        // Configuration has been set up by the framework via WebHost.CreateDefaultBuilder
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            // Note that this code is also initiated by the MatchAlg Validation tests when running a virtual Server.
            // When it does so, the appSettings file that is being looked at is the one *in the Validation folder*,
            // so anything that this project looks for needs to be defined over there, as well as in this project's
            // appSettings file.
            // (Comment duplicated in Validation.ServiceConfiguration, Validation.appSettings, Api.Startup (twice), Api.appSettings)
            if (!env.ContentRootPath.Contains("Test"))
            {
                var builder = new ConfigurationBuilder();
                builder.AddConfiguration(configuration);
                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                builder.AddUserSecrets(Assembly.GetExecutingAssembly());
                this.configuration = builder.Build();
            }
            else
            {
                this.configuration = configuration;
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterSettings(services);
            services.RegisterSearch(OptionsReaderFor<ApplicationInsightsSettings>(),
                OptionsReaderFor<AzureStorageSettings>(),
                OptionsReaderFor<HlaMetadataDictionarySettings>(),
                OptionsReaderFor<MacDictionarySettings>(),
                OptionsReaderFor<MessagingServiceBusSettings>(),
                OptionsReaderFor<NotificationsServiceBusSettings>(),
                OptionsReaderFor<MatchingConfigurationSettings>(),
                ConnectionStringReader("PersistentSql"),
                ConnectionStringReader("SqlA"),
                ConnectionStringReader("SqlB"),
                ConnectionStringReader("DonorSql"));
            
            services.RegisterDataRefresh(
                OptionsReaderFor<AzureAuthenticationSettings>(),
                OptionsReaderFor<AzureDatabaseManagementSettings>(),
                OptionsReaderFor<DataRefreshSettings>(),
                OptionsReaderFor<ApplicationInsightsSettings>(),
                OptionsReaderFor<AzureStorageSettings>(),
                OptionsReaderFor<HlaMetadataDictionarySettings>(),
                OptionsReaderFor<MacDictionarySettings>(),
                OptionsReaderFor<MessagingServiceBusSettings>(),
                OptionsReaderFor<NotificationsServiceBusSettings>(),
                OptionsReaderFor<DonorManagementSettings>(),
                ConnectionStringReader("PersistentSql"),
                ConnectionStringReader("SqlA"),
                ConnectionStringReader("SqlB"),
                ConnectionStringReader("DonorSql"));
            
            services.RegisterDonorManagement(
                OptionsReaderFor<ApplicationInsightsSettings>(),
                OptionsReaderFor<AzureStorageSettings>(),
                OptionsReaderFor<DonorManagementSettings>(),
                OptionsReaderFor<HlaMetadataDictionarySettings>(),
                OptionsReaderFor<MacDictionarySettings>(),
                OptionsReaderFor<MessagingServiceBusSettings>(),
                OptionsReaderFor<NotificationsServiceBusSettings>(),
                ConnectionStringReader("PersistentSql"),
                ConnectionStringReader("SqlA"),
                ConnectionStringReader("SqlB"));

            services.ConfigureSwaggerService();

            services
                .AddMvc(options => { options.EnableEndpointRouting = false; })
                // When using the default System.Text.Json, all properties in `LocusPositionScoreDetails` models were ignored when serialising
                // Until the cause for this has been identified and eliminated, Newtonsoft.Json must be used instead.
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.ConfigureSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private static void RegisterSettings(IServiceCollection services)
        {
            // Note that this code is also initiated by the MatchAlg Validation tests when running a virtual Server.
            // When it does so, the appSettings file that is being looked at is the one *in the Validation folder*,
            // so all these properties that are being looked for need to be defined over there, as well as in this
            // project's appSettings file.
            // (Comment duplicated in Validation.ServiceConfiguration, Validation.appSettings, Api.Startup (twice), Api.appSettings)
            services.RegisterAsOptions<ApplicationInsightsSettings>("ApplicationInsights");
            services.RegisterAsOptions<AzureAuthenticationSettings>("AzureManagement:Authentication");
            services.RegisterAsOptions<AzureDatabaseManagementSettings>("AzureManagement:Database");
            services.RegisterAsOptions<AzureStorageSettings>("AzureStorage");
            services.RegisterAsOptions<DataRefreshSettings>("DataRefresh");
            services.RegisterAsOptions<DonorManagementSettings>("DataRefresh:DonorManagement");
            services.RegisterAsOptions<HlaMetadataDictionarySettings>("HlaMetadataDictionary");
            services.RegisterAsOptions<MacDictionarySettings>("MacDictionary");
            services.RegisterAsOptions<MatchingConfigurationSettings>("MatchingConfiguration");
            services.RegisterAsOptions<MessagingServiceBusSettings>("MessagingServiceBus");
            services.RegisterAsOptions<NotificationsServiceBusSettings>("NotificationsServiceBus");
        }
    }
}