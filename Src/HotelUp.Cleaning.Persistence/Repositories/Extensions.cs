using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Cleaning.Persistence.Repositories;

public static class Extensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<ICleanerRepository, CleanerRepository>();
        services.AddScoped<ICleaningTaskRepository, CleaningTaskRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        return services;
    }
}