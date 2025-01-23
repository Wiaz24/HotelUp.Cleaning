using HotelUp.Cleaning.Services.Services;
using HotelUp.Customer.Application.Events;
using MassTransit;

namespace HotelUp.Cleaning.Services.Events.External.Handlers;

public class ReservationCanceledEventHandler : IConsumer<ReservationCanceledEvent>
{
    private readonly ICleaningTaskService _cleaningTaskService;

    public ReservationCanceledEventHandler(ICleaningTaskService cleaningTaskService)
    {
        _cleaningTaskService = cleaningTaskService;
    }

    public Task Consume(ConsumeContext<ReservationCanceledEvent> context)
    {
        var reservationCanceledEvent = context.Message;
        return _cleaningTaskService.RemoveCleaningTasksForReservation(reservationCanceledEvent);
    }
}