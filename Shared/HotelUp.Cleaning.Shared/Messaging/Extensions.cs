using System.Reflection;
using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using HotelUp.Cleaning.Shared.Messaging.RabbitMQ;

namespace HotelUp.Cleaning.Shared.Messaging;

internal static class Extensions
{
    internal static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        var assembliesWithConsumers = GetAssembliesWithConsumers();
        services.AddRabbitMq();
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            if (assembliesWithConsumers.Length > 0)
            {
                busConfigurator.AddConsumers(assembliesWithConsumers);
            }
            busConfigurator.UsingRabbitMq((context, rabbitMqConfigurator) =>
            {
                rabbitMqConfigurator.MessageTopology.SetEntityNameFormatter(new CustomNameFormatter());

                rabbitMqConfigurator.ConfigureJsonSerializerOptions(serializerOptions =>
                {
                    serializerOptions.Converters.Add(new JsonStringEnumConverter());
                    return serializerOptions;
                });
                var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                rabbitMqConfigurator.Host(options.Host, hostConfigurator =>
                {
                    hostConfigurator.Username(options.UserName);
                    hostConfigurator.Password(options.Password);
                });
                rabbitMqConfigurator.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    private static Assembly[] GetAssembliesWithConsumers()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.GetInterfaces()
                            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>))
                        && p.IsClass
                        && !p.IsAbstract)
            .Select(p => p.Assembly)
            .Distinct()
            .Where(a => a.FullName?.Contains("MassTransit") == false)
            .ToArray();
        return assemblies;
    }
}