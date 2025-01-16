namespace HotelUp.Cleaning.API.Cors;

public static class Extensions
{
    private const string FrontendPolicy = "Frontend";
    private const string SectionName = "AllowedOrigins";

    public static IServiceCollection AddCorsForFrontend(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedHosts = configuration.GetSection(SectionName).Get<string[]>()
                           ?? throw new NullReferenceException($"{SectionName} not found in config file");

        services.AddCors(
            options => options.AddPolicy(
                FrontendPolicy,
                policy => policy
                    .WithOrigins(allowedHosts)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            ));

        return services;
    }

    public static IApplicationBuilder UseCorsForFrontend(this IApplicationBuilder app)
    {
        app.UseCors(FrontendPolicy);
        return app;
    }
}