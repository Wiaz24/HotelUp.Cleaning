using HotelUp.Cleaning.Persistence.EF;
using HotelUp.Cleaning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelUp.Cleaning.Persistence.Repositories;

public class CleanerRepository : ICleanerRepository
{
    private readonly AppDbContext _dbContext;

    public CleanerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public Task<Cleaner?> GetAsync(Guid id)
    {
        return _dbContext.Cleaners
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Cleaner cleaner)
    {
        await _dbContext.Cleaners.AddAsync(cleaner);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Cleaner cleaner)
    {
        _dbContext.Cleaners.Remove(cleaner);
        await _dbContext.SaveChangesAsync();
    }
}