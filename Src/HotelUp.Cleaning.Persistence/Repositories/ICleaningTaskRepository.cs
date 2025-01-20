using HotelUp.Cleaning.Persistence.Entities;

namespace HotelUp.Cleaning.Persistence.Repositories;

public interface ICleaningTaskRepository
{
    Task<CleaningTask?> GetByIdAsync(Guid id);
    Task<List<CleaningTask>> GetByCleanerIdAsync(Guid id);
    Task<List<CleaningTask>> GetByReservationIdAsync(Guid id);
    Task AddAsync(CleaningTask task);
    Task AddRangeAsync(IEnumerable<CleaningTask> tasks);
    Task UpdateAsync(CleaningTask task);
    Task DeleteAsync(CleaningTask task);
}