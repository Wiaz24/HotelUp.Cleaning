using HotelUp.Cleaning.Persistence.Const;
using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Cleaning.Persistence.Repositories;
using HotelUp.Cleaning.Services.Services.Exceptions;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;

namespace HotelUp.Cleaning.Services.Services;

public class CleaningTaskService : ICleaningTaskService
{
    private readonly ICleaningTaskRepository _cleaningTaskRepository;
    private readonly ICleanerRepository _cleanerRepository;
    private readonly TimeProvider _timeProvider;

    public CleaningTaskService(ICleaningTaskRepository cleaningTaskRepository, 
        TimeProvider timeProvider, ICleanerRepository cleanerRepository)
    {
        _cleaningTaskRepository = cleaningTaskRepository;
        _timeProvider = timeProvider;
        _cleanerRepository = cleanerRepository;
    }

    public async Task<CleaningTask?> GetTaskByIdAsync(Guid id)
    {
        return await _cleaningTaskRepository.GetByIdAsync(id);
    }

    public async Task<Guid> CreateOnDemandAsync(Guid reservationId, DateTime realisationDate, int roomNumber)
    {
        if (realisationDate < _timeProvider.GetUtcNow())
        {
            throw new InvalidRealisationDateException();
        }

        if (await _cleaningTaskRepository.ReservationExistsAsync(reservationId) is false)
        {
            throw new ReservationNotFoundException(reservationId);
        }

        var cleaner = await _cleanerRepository.getOneWithLeastTasksAsync();
        if (cleaner is null)
        {
            throw new NoCleanersAvailableException();
        }
        
        var task = new CleaningTask
        {
            Id = Guid.NewGuid(),
            ReservationId = reservationId,
            RealisationDate = realisationDate,
            RoomNumber = roomNumber,
            Status = TaskStatus.Pending,
            CleaningType = CleaningType.OnDemand,
            Cleaner = cleaner
        };
        
        await _cleaningTaskRepository.AddAsync(task);
        return task.Id;
    }
}