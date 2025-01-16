using HotelUp.Cleaning.Persistence.EF.Health;
using HotelUp.Cleaning.Persistence.EF.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Cleaning.Persistence.EF;

internal static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigurePostgres();
        services.AddPostgres<AppDbContext>();
        services.AddHostedService<DatabaseInitializer>();
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database");
        return services;
    }
}