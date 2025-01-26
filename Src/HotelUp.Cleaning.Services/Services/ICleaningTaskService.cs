using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Cleaning.Services.DTOs;
using HotelUp.Cleaning.Services.Events;
using HotelUp.Cleaning.Services.Events.External;
using HotelUp.Customer.Application.Events;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;

namespace HotelUp.Cleaning.Services.Services;

public interface ICleaningTaskService
{
    Task<CleaningTaskDto?> GetTaskByIdAsync(Guid id);
    Task<IEnumerable<CleaningTaskDto>> GetTasksByCleanerIdAsync(Guid cleanerId);
    Task<Guid> CreateOnDemandAsync(Guid reservationId, DateOnly realisationDate, int roomNumber);
    Task UpdateStatusAsync(Guid cleaningTaskId, Guid cleanerId, TaskStatus status);
    Task CreateCleaningTasksForReservation(ReservationCreatedEvent reservation);
    Task RemoveCleaningTasksForReservation(ReservationCanceledEvent reservation);
}