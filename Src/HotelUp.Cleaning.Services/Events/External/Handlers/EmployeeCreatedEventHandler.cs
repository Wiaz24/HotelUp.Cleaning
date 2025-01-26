using HotelUp.Cleaning.Services.Events.External.DTOs;
using HotelUp.Cleaning.Services.Services;
using HotelUp.Employee.Services.Events;
using MassTransit;

namespace HotelUp.Cleaning.Services.Events.External.Handlers;

public class EmployeeCreatedEventHandler : IConsumer<EmployeeCreatedEvent>
{
    private readonly ICleanerService _cleanerService;

    public EmployeeCreatedEventHandler(ICleanerService cleanerService)
    {
        _cleanerService = cleanerService;
    }

    public async Task Consume(ConsumeContext<EmployeeCreatedEvent> context)
    {
        var message = context.Message;
        if (message.Role == EmployeeType.Cleaner)
        {
            await _cleanerService.CreateAsync(message.Id);
        }
    }
}