using HotelUp.Cleaning.Persistence.Entities;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;

namespace HotelUp.Cleaning.Services.Services;

public interface ICleaningTaskService
{
    Task<CleaningTask?> GetTaskByIdAsync(Guid id);
    Task<IEnumerable<CleaningTask>> GetTasksByCleanerIdAsync(Guid cleanerId);
    Task<Guid> CreateOnDemandAsync(Guid reservationId, DateTime realisationDate, int roomNumber);
    Task UpdateStatusAsync(Guid cleaningTaskId, Guid cleanerId, TaskStatus status);
}