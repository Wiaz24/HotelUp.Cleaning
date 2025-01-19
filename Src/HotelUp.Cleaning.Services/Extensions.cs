using HotelUp.Cleaning.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Cleaning.Services;

public static class Extensions
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddApplicationServices();
        return services;
    }
}