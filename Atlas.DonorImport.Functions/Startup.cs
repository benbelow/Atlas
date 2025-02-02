using Atlas.Common.ApplicationInsights;
using Atlas.Common.Notifications;
using Atlas.DonorImport.ExternalInterface.DependencyInjection;
using Atlas.DonorImport.ExternalInterface.Settings;
using Atlas.DonorImport.ExternalInterface.Settings.ServiceBus;
using Atlas.DonorImport.Functions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using static Atlas.Common.Utils.Extensions.DependencyInjectionUtils;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Atlas.DonorImport.Functions
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            RegisterSettings(builder.Services);
            builder.Services.RegisterDonorImport(
                OptionsReaderFor<ApplicationInsightsSettings>(),
                OptionsReaderFor<MessagingServiceBusSettings>(),
                OptionsReaderFor<NotificationConfigurationSettings>(),
                OptionsReaderFor<NotificationsServiceBusSettings>(),
                OptionsReaderFor<StalledFileSettings>(),
                ConnectionStringReader("DonorStoreSql")
            );
        }

        private static void RegisterSettings(IServiceCollection services)
        {
            services.RegisterAsOptions<ApplicationInsightsSettings>("ApplicationInsights");
            services.RegisterAsOptions<MessagingServiceBusSettings>("MessagingServiceBus");
            services.RegisterAsOptions<NotificationConfigurationSettings>("NotificationConfiguration");
            services.RegisterAsOptions<NotificationsServiceBusSettings>("NotificationsServiceBus");
            services.RegisterAsOptions<StalledFileSettings>("DonorImport");
        }
    }
}