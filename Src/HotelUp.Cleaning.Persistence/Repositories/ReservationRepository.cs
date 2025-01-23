using HotelUp.Cleaning.Persistence.EF;
using HotelUp.Cleaning.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelUp.Cleaning.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _dbContext;

    public ReservationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Reservation?> GetByIdAsync(Guid id)
    {
        return _dbContext.Reservations
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return _dbContext.Reservations
            .AnyAsync(x => x.Id == id);
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Reservation reservation)
    {
        _dbContext.Reservations.Update(reservation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Reservation reservation)
    {
        _dbContext.Reservations.Remove(reservation);
        await _dbContext.SaveChangesAsync();
    }
}