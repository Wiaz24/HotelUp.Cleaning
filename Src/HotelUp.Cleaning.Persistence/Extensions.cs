using HotelUp.Cleaning.Persistence.EF;
using HotelUp.Cleaning.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Cleaning.Persistence;

public static class Extensions
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddRepositories();
        return services;
    }
}