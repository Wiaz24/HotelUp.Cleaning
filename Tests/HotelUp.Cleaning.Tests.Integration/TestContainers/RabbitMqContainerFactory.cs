using System.Text.RegularExpressions;
using DotNet.Testcontainers.Builders;
using Testcontainers.RabbitMq;

namespace HotelUp.Cleaning.Tests.Integration.TestContainers;

internal static class RabbitMqContainerFactory
{
    private const int DefaultAmqpPort = 5672;
    private static int _numInstances = 0;
    private static int GetContainerInstance => Interlocked.Increment(ref _numInstances) - 1;
    internal static RabbitMqContainer Create()
    {
        var instance = GetContainerInstance;
        var hostAmqpPort = DefaultAmqpPort + instance + 1;
        return new RabbitMqBuilder()
            .WithImage("rabbitmq:management")
            .WithPortBinding(hostAmqpPort, DefaultAmqpPort)
            .WithEnvironment("RABBITMQ_DEFAULT_USER", "guest")
            .WithEnvironment("RABBITMQ_DEFAULT_PASS", "guest")
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilMessageIsLogged(new Regex("started TCP listener on \\[::\\]:")))
            .Build();
    }
}