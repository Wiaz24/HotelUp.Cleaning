using System.Net.Http.Headers;
using System.Security.Claims;
using HotelUp.Customer.Tests.Integration.Utils;
using Xunit.Abstractions;

namespace HotelUp.Cleaning.Tests.Integration;

public abstract class IntegrationTestsBase : IClassFixture<TestWebAppFactory>
{
    protected readonly IServiceProvider ServiceProvider;
    protected readonly ITestOutputHelper TestOutputHelper;

    protected IntegrationTestsBase(TestWebAppFactory factory, ITestOutputHelper testOutputHelper)
    {
        Factory = factory;
        TestOutputHelper = testOutputHelper;
        ServiceProvider = factory.Services;
    }

    protected TestWebAppFactory Factory { get; }

    protected HttpClient CreateHttpClientWithToken(Guid clientId, IEnumerable<Claim> claims)
    {
        var httpClient = Factory.CreateClient();
        var token = MockJwtTokens.GenerateJwtToken(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, clientId.ToString())
        }.Concat(claims));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return httpClient;
    }
}