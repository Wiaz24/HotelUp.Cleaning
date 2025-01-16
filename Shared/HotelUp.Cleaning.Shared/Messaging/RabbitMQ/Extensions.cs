using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Cleaning.Shared.Messaging.RabbitMQ;

internal static class Extensions
{
    private const string SectionName = "MessageBroker:RabbitMQ";

    internal static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddOptions<RabbitMqOptions>()
            .BindConfiguration(SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services;
    }
}