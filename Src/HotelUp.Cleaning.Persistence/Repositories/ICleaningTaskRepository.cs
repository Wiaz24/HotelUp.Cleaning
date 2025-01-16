using HotelUp.Cleaning.Persistence.Entities;

namespace HotelUp.Cleaning.Persistence.Repositories;

public interface ICleaningTaskRepository
{
    Task<CleaningTask?> GetAsync(Guid id);
    Task<List<CleaningTask>> GetByCleanerIdAsync(Guid id);
    Task AddAsync(CleaningTask task);
    Task UpdateAsync(CleaningTask task);
    Task DeleteAsync(CleaningTask task);
}