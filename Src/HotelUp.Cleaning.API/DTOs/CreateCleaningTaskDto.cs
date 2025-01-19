using System.ComponentModel;

namespace HotelUp.Cleaning.API.DTOs;

public record CreateCleaningTaskDto
{
    public required Guid ReservationId { get; init; }
    
    [DefaultValue("2025-01-01")]
    public required DateTime RealisationDate { get; init; }
    
    [DefaultValue(1)]
    public required int RoomNumber { get; init; }
}