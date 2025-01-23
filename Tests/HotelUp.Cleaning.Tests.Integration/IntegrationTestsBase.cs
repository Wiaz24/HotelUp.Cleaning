namespace HotelUp.Cleaning.Tests.Integration;

public abstract class IntegrationTestsBase : IClassFixture<TestWebAppFactory>
{
    protected TestWebAppFactory Factory { get; }
    protected readonly IServiceProvider ServiceProvider;
    
    protected IntegrationTestsBase(TestWebAppFactory factory)
    {
        Factory = factory;
        ServiceProvider = factory.Services;
    }
}