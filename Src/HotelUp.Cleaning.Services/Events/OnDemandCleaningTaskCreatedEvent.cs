namespace HotelUp.Cleaning.Services.Events;

public record OnDemandCleaningTaskCreatedEvent
{
    public Guid TaskId { get; init; }
    public Guid ReservationId { get; init; }
}