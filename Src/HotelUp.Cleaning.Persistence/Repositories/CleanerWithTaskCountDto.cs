using HotelUp.Cleaning.Persistence.Entities;

namespace HotelUp.Cleaning.Persistence.Repositories;

public class CleanerWithTaskCountDto
{
    public CleanerWithTaskCountDto(Cleaner cleaner, int taskCount)
    {
        Cleaner = cleaner;
        TaskCount = taskCount;
    }

    public Cleaner Cleaner { get; set; }
    public int TaskCount { get; set; }
}