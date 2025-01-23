using HotelUp.Cleaning.Persistence.Entities;

namespace HotelUp.Cleaning.Services.Services;

public interface ICleanerService
{
    Task<Cleaner?> GetByIdAsync(Guid id);
    Task CreateAsync(Guid id);
}