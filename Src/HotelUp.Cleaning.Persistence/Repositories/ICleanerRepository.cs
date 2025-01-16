using HotelUp.Cleaning.Persistence.Entities;

namespace HotelUp.Cleaning.Persistence.Repositories;

public interface ICleanerRepository
{
    Task<Cleaner?> GetAsync(Guid id);
    Task AddAsync(Cleaner cleaner);
    Task RemoveAsync(Cleaner cleaner);
}