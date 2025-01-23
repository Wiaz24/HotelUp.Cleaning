using HotelUp.Cleaning.Persistence.EF;
using HotelUp.Cleaning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HotelUp.Cleaning.Persistence.Repositories;

public class CleanerRepository : ICleanerRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public CleanerRepository(AppDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }


    public async Task<Cleaner?> GetAsync(Guid id)
    {
        var cacheKey = $"Cleaner_{id}";
        var cachedResult = _memoryCache.Get<Cleaner>(cacheKey);
        if (cachedResult is not null)
        {
            return cachedResult;
        }
        var result = await _dbContext.Cleaners
            .FirstOrDefaultAsync(x => x.Id == id);
        _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    public async Task<Cleaner?> getOneWithLeastTasksAsync()
    {
        return await _dbContext.Cleaners
            .Select(c => new
            {
                Cleaner = c,
                TaskCount = c.CleaningTasks.Count
            })
            .OrderBy(c => c.TaskCount)
            .Select(x => x.Cleaner)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CleanerWithTaskCountDto>> GetCleanersOrderedByTaskCountAsync()
    {
        var cleanersWithTaskCounts = await _dbContext.Cleaners
            .Select(c => new
            {
                Cleaner = c,
                TaskCount = c.CleaningTasks.Count
            })
            .OrderBy(c => c.TaskCount)
            .ToListAsync();
        return cleanersWithTaskCounts
            .Select(x => new CleanerWithTaskCountDto(x.Cleaner, x.TaskCount));
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