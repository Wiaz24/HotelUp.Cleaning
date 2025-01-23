using HotelUp.Cleaning.Persistence.EF;
using HotelUp.Cleaning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HotelUp.Cleaning.Persistence.Repositories;

public class CleaningTaskRepository : ICleaningTaskRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public CleaningTaskRepository(AppDbContext dbContext, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _dbContext = dbContext;
    }

    public async Task<CleaningTask?> GetByIdAsync(Guid id)
    {
        var cacheKey = $"CleaningTask_{id}";
        var cachedResult = _memoryCache.Get<CleaningTask>(cacheKey);
        if (cachedResult is not null)
        {
            return cachedResult;
        }
        var result = await _dbContext.CleaningTasks
            .Include(x => x.Cleaner)
            .FirstOrDefaultAsync(x => x.Id == id);
        _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    public async Task<List<CleaningTask>> GetByCleanerIdAsync(Guid id)
    {
        var cacheKey = $"CleaningTask_Cleaner_{id}";
        var cachedResult = _memoryCache.Get<List<CleaningTask>>(cacheKey);
        if (cachedResult is not null)
        {
            return cachedResult;
        }
        var result = await _dbContext.CleaningTasks
            .Include(x => x.Cleaner)
            .Where(x => x.Cleaner.Id == id)
            .ToListAsync();
        _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    public Task<List<CleaningTask>> GetByReservationIdAsync(Guid id)
    {
        return _dbContext.CleaningTasks
            .Where(x => x.ReservationId == id)
            .ToListAsync();
    }

    public async Task AddAsync(CleaningTask task)
    {
        await _dbContext.CleaningTasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();
    }

    public Task AddRangeAsync(IEnumerable<CleaningTask> tasks)
    {
        _dbContext.CleaningTasks.AddRange(tasks);
        return _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(CleaningTask task)
    {
        _dbContext.CleaningTasks.Update(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(CleaningTask task)
    {
        _dbContext.CleaningTasks.Remove(task);
        await _dbContext.SaveChangesAsync();
    }

    public Task DeleteRangeAsync(IEnumerable<CleaningTask> tasks)
    {
        _dbContext.CleaningTasks.RemoveRange(tasks);
        return _dbContext.SaveChangesAsync();
    }
}