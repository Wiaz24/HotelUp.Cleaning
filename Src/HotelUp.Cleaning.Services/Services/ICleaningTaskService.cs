using HotelUp.Cleaning.Persistence.Entities;

namespace HotelUp.Cleaning.Services.Services;

public interface ICleaningTaskService
{
    Task<CleaningTask?> GetTaskByIdAsync(Guid id);
    Task<Guid> CreateOnDemandAsync(Guid reservationId, DateTime realisationDate, int roomNumber);
}