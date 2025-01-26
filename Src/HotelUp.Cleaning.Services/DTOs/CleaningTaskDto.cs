using HotelUp.Cleaning.Persistence.Const;
using HotelUp.Cleaning.Persistence.Entities;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;

namespace HotelUp.Cleaning.Services.DTOs;

public record CleaningTaskDto
{
    public Guid Id { get; init; }
    public Guid ReservationId { get; init; }
    public DateTime RealisationDate { get; init; }
    public int RoomNumber { get; init; }
    public TaskStatus Status { get; set; }
    public CleaningType CleaningType { get; init; }
    public Guid CleanerId { get; set; }
    
    public static CleaningTaskDto FromEntity(CleaningTask cleaningTask) => new()
    {
        Id = cleaningTask.Id,
        ReservationId = cleaningTask.ReservationId,
        RealisationDate = cleaningTask.RealisationDate,
        RoomNumber = cleaningTask.RoomNumber,
        Status = cleaningTask.Status,
        CleaningType = cleaningTask.CleaningType,
        CleanerId = cleaningTask.CleanerId
    };
}