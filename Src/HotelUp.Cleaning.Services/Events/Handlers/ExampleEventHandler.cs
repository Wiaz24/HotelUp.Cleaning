using MassTransit;
using Microsoft.Extensions.Logging;

namespace HotelUp.Cleaning.Services.Events.Handlers;

public class ExampleEventHandler : IConsumer<ExampleEvent>
{
    private readonly ILogger<ExampleEventHandler> _logger;

    public ExampleEventHandler(ILogger<ExampleEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ExampleEvent> context)
    {
        _logger.LogInformation("Received event: {Event}", context.Message);
        return Task.CompletedTask;
    }
}