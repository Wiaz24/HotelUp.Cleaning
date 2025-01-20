


namespace HotelUp.Cleaning.Persistence.Entities;

public class Cleaner
{
    public Guid Id { get; init; }
    public required List<CleaningTask> CleaningTasks { get; init; }
}