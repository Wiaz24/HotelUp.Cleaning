using HotelUp.Cleaning.Services.Services;
using HotelUp.Customer.Application.Events;
using MassTransit;

namespace HotelUp.Cleaning.Services.Events.External.Handlers;

public class ReservationCreatedEventHandler : IConsumer<ReservationCreatedEvent>
{
    private readonly ICleaningTaskService _cleaningTaskService;

    public ReservationCreatedEventHandler(ICleaningTaskService cleaningTaskService)
    {
        _cleaningTaskService = cleaningTaskService;
    }

    public Task Consume(ConsumeContext<ReservationCreatedEvent> context)
    {
        var reservation = context.Message;
        return _cleaningTaskService.CreateCleaningTasksForReservation(reservation);
    }
}