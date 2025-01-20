
using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Cleaning.Persistence.Repositories;

namespace HotelUp.Cleaning.Services.Services;

public class CleanerService : ICleanerService
{
    private readonly ICleanerRepository _cleanerRepository;

    public CleanerService(ICleanerRepository cleanerRepository)
    {
        _cleanerRepository = cleanerRepository;
    }

    public Task<Cleaner?> GetByIdAsync(Guid id)
    {
        return _cleanerRepository.GetAsync(id);
    }

    public Task CreateAsync(Guid id)
    {
        var cleaner = new Cleaner
        {
            Id = id,
            CleaningTasks = []
        };
        return _cleanerRepository.AddAsync(cleaner);
    }
}