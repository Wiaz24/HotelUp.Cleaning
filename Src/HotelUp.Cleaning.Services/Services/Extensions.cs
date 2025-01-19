using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Cleaning.Services.Services;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICleaningTaskService, CleaningTaskService>();
        services.AddScoped<ICleanerService, CleanerService>();
        return services;
    }
    
}