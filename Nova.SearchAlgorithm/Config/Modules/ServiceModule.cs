﻿using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.ApplicationInsights;
using Nova.SearchAlgorithm.Repositories.SearchRequests.AzureStorage;
using Nova.Utils.ApplicationInsights;
using Nova.Utils.Auth;
using Nova.Utils.Solar;
using Nova.Utils.WebApi.ApplicationInsights;
using Nova.Utils.WebApi.Filters;
using Module = Autofac.Module;

namespace Nova.SearchAlgorithm.Config.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterInstance(AutomapperConfig.CreateMapper())
                .SingleInstance()
                .AsImplementedInterfaces();

            builder.RegisterType<Repositories.SearchRequests.SearchRequestRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<Services.SearchRequestService>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<AppSettingsApiKeyProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ApiKeyRequiredAttribute>().AsSelf().SingleInstance();

            builder.RegisterType<CloudTableFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SolarConnectionFactory>().AsImplementedInterfaces().SingleInstance();

            var solarSettings = new SolarConnectionSettings
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SolarConnectionString"].ConnectionString
            };
            builder.RegisterInstance(solarSettings).AsSelf().SingleInstance();

            var logger = new RequestAwareLogger(new TelemetryClient(),
                ConfigurationManager.AppSettings["insights.logLevel"].ToLogLevel());
            builder.RegisterInstance(logger).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}