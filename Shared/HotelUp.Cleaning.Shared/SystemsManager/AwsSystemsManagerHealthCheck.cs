using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HotelUp.Cleaning.Shared.SystemsManager;

public class AwsSystemsManagerHealthCheck : IHealthCheck
{
    private readonly IAmazonSimpleSystemsManagement _ssmClient;

    public AwsSystemsManagerHealthCheck(IAmazonSimpleSystemsManagement ssmClient)
    {
        _ssmClient = ssmClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var response = await _ssmClient.GetDocumentAsync(new GetDocumentRequest()
            {
                Name = "AWS-ASGEnterStandby",
                DocumentVersion = "1"
            }, cancellationToken);

            if (response?.Content != null)
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}