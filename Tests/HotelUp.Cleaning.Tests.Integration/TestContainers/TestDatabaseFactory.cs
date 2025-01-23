using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;

namespace HotelUp.Cleaning.Tests.Integration.TestContainers;

internal static class TestDatabaseFactory
{
    private const int DefaultPort = 5432;
    private static int _numInstances = 0;
    // Container port starts from 5433 to avoid conflicts with local Postgres
    private static int GetPort => DefaultPort + Interlocked.Increment(ref _numInstances);
    internal static PostgreSqlContainer Create()
    {
        var port = GetPort;
        return new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("TestDb")
            .WithPortBinding(port, DefaultPort)
            .WithUsername("Postgres")
            .WithPassword("Postgres")
            .WithCommand("-c", "track_commit_timestamp=true")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(DefaultPort))
            .Build();
    }
}