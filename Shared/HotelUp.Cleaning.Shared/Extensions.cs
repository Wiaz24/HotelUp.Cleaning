using HotelUp.Cleaning.Shared.Auth;
using HotelUp.Cleaning.Shared.Logging;
using HotelUp.Cleaning.Shared.Messaging;
using HotelUp.Cleaning.Shared.SystemsManager;
using HealthChecks.UI.Client;
using HotelUp.Cleaning.Shared.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Cleaning.Shared;

public static class Extensions
{
    public static WebApplicationBuilder AddShared(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();
        builder.Services.AddAuth(builder.Configuration);
        builder.Services.AddHttpClient();
        builder.Services.AddMessaging();
        builder.AddCustomSystemsManagers();
        builder.Services.AddTransient<ExceptionMiddleware>();
        builder.AddCustomLogging();
        return builder;
    }

    public static IApplicationBuilder UseShared(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHealthChecks("/api/cleaning/_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseAuth();
        return app;
    }
}