using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HotelUp.Cleaning.Persistence.EF.Postgres;

public static class Extensions
{
    private const string SectionName = "Postgres";

    public static IServiceCollection ConfigurePostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetSection(SectionName));

        return services;
    }

    public static IServiceCollection AddPostgres<T>(this IServiceCollection services) where T : DbContext
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<PostgresOptions>>();
        var connectionString = options.Value.ConnectionString;
        services.AddDbContext<T>(x => x.UseNpgsql(connectionString));
        return services;
    }
}