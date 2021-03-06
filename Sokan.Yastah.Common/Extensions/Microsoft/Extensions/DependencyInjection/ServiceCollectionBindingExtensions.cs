﻿using System.Reflection;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionBindingExtensions
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services,
            Assembly assembly,
            IConfiguration configuration)
        {
            foreach (var serviceDescriptor in ServiceBindingAttribute.EnumerateServiceDescriptors(assembly))
                services.Add(serviceDescriptor);

            foreach (var serviceConfigurator in ServiceConfiguratorAttribute.EnumerateServiceConfigurators(assembly))
                serviceConfigurator.ConfigureServices(services, configuration);

            return services;
        }

        public static IServiceCollection AddSingleton<TService1, TService2, TImplementation>(
                    this IServiceCollection services)
                where TService1 : class
                where TService2 : class
                where TImplementation : class, TService1, TService2
            => services
                .AddSingleton<TImplementation>()
                .AddSingleton<TService1>(serviceProvider => serviceProvider.GetRequiredService<TImplementation>())
                .AddSingleton<TService2>(serviceProvider => serviceProvider.GetRequiredService<TImplementation>());
    }
}
