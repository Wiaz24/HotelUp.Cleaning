using Amazon.Extensions.Configuration.SystemsManager;
using Amazon.SimpleSystemsManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Cleaning.Shared.SystemsManager;

internal static class Extensions
{
    internal static WebApplicationBuilder AddCustomSystemsManagers(this WebApplicationBuilder builder)
    {
        var environmentName = builder.Environment.EnvironmentName;
        builder.Configuration.AddSystemsManager(configuration =>
        {
            configuration.AwsOptions = builder.Configuration.GetAWSOptions();
            configuration.Path = $"/HotelUp.Cleaning/{environmentName}";
            configuration.ReloadAfter = TimeSpan.FromMinutes(5);
        });
        builder.Services.AddAWSService<IAmazonSimpleSystemsManagement>();
        builder.Services.AddTransient<AwsSystemsManagerHealthCheck>();
        builder.Services.AddHealthChecks()
            .AddCheck<AwsSystemsManagerHealthCheck>("AWS Systems Manager");
        return builder;
    }
}