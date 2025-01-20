using HotelUp.Cleaning.Persistence.Const;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;


namespace HotelUp.Cleaning.Persistence.Entities;

public class CleaningTask
{
    public Guid Id { get; init; }
    public required Reservation Reservation { get; init; }
    public DateTime RealisationDate { get; init; }
    public int RoomNumber { get; init; }
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public CleaningType CleaningType { get; init; }
    public Cleaner? Cleaner { get; set; }
}