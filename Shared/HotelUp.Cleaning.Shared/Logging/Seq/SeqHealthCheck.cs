using System.Net;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace HotelUp.Cleaning.Shared.Logging.Seq;

public class SeqHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly SeqOptions _seqOptions;

    public SeqHealthCheck(IHttpClientFactory httpClientFactory, IOptions<SeqOptions> seqOptions)
    {
        _seqOptions = seqOptions.Value;
        _httpClient = httpClientFactory.CreateClient("Seq");
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await _httpClient.GetAsync(_seqOptions.ServerUrl, cancellationToken);
        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            return HealthCheckResult.Healthy();
        }
        else
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}