using HotelUp.Cleaning.Persistence.EF;
using HotelUp.Cleaning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelUp.Cleaning.Persistence.Repositories;

public class CleaningTaskRepository : ICleaningTaskRepository
{
    private readonly AppDbContext _dbContext;

    public CleaningTaskRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CleaningTask?> GetAsync(Guid id)
    {
        return _dbContext.CleaningTasks
            .Include(x => x.Cleaner)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<CleaningTask>> GetByCleanerIdAsync(Guid id)
    {
        return _dbContext.CleaningTasks
            .Include(x => x.Cleaner)
            .Where(x => x.Cleaner.Id == id)
            .ToListAsync();
    }

    public async Task AddAsync(CleaningTask task)
    {
        await _dbContext.CleaningTasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();
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
}