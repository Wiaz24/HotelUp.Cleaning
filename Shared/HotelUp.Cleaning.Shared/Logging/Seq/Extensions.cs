using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Cleaning.Shared.Logging.Seq;

internal static class Extensions
{
    private const string SectionName = "seq";

    internal static IServiceCollection AddSeqLogging(this IServiceCollection services)
    {
        services.AddOptions<SeqOptions>()
            .BindConfiguration(SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHealthChecks()
            .AddCheck<SeqHealthCheck>("Seq");
        return services;
    }
}