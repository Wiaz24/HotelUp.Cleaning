using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HotelUp.Cleaning.Shared.Auth;

internal static class Extensions
{
    private const string OidcSectionName = "Oidc";

    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<OidcOptions>()
            .BindConfiguration(OidcSectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var oidcOptions = configuration.GetSection(OidcSectionName).Get<OidcOptions>()
                          ?? throw new NullReferenceException("OIDC options are missing in appsettings.json");

        services.AddHealthChecks()
            .AddCheck<OidcProviderHealthCheck>("oidc_provider");
        services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.MetadataAddress = oidcOptions.MetadataAddress;
                options.RequireHttpsMetadata = oidcOptions.RequireHttpsMetadata;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = oidcOptions.ValidateIssuer,
                    ValidateAudience = oidcOptions.ValidateAudience,
                    ValidateLifetime = oidcOptions.ValidateLifetime,
                    ValidateIssuerSigningKey = oidcOptions.ValidateIssuerSigningKey
                };
            });
        services.AddAuthorization();
        return services;
    }

    internal static IApplicationBuilder UseAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}