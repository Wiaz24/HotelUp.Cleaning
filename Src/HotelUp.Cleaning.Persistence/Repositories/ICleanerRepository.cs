using HotelUp.Cleaning.Persistence.Entities;

namespace HotelUp.Cleaning.Persistence.Repositories;

public interface ICleanerRepository
{
    Task<Cleaner?> GetAsync(Guid id);
    Task<Cleaner?> getOneWithLeastTasksAsync();
    Task AddAsync(Cleaner cleaner);
    Task RemoveAsync(Cleaner cleaner);
}