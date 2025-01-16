using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace HotelUp.Cleaning.Shared.Auth;

public class OidcProviderHealthCheck : IHealthCheck
{
    private readonly OidcOptions _oidcOptions;
    private readonly HttpClient _httpClient;

    public OidcProviderHealthCheck(IOptionsSnapshot<OidcOptions> oidcOptions, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Oidc");
        _oidcOptions = oidcOptions.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await _httpClient.GetAsync(_oidcOptions.MetadataAddress, cancellationToken);
        if (result.IsSuccessStatusCode)
        {
            return HealthCheckResult.Healthy();
        }
        else
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}