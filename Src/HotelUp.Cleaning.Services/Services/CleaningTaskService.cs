using HotelUp.Cleaning.Persistence.Const;
using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Cleaning.Persistence.Repositories;
using HotelUp.Cleaning.Services.Events;
using HotelUp.Cleaning.Services.Events.External.DTOs;
using HotelUp.Cleaning.Services.Services.Exceptions;
using HotelUp.Cleaning.Shared.Exceptions;
using HotelUp.Customer.Application.Events;
using MassTransit;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;

namespace HotelUp.Cleaning.Services.Services;

public class CleaningTaskService : ICleaningTaskService
{
    private readonly ICleaningTaskRepository _cleaningTaskRepository;
    private readonly ICleanerRepository _cleanerRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IPublishEndpoint _bus;

    public CleaningTaskService(ICleaningTaskRepository cleaningTaskRepository, 
        ICleanerRepository cleanerRepository, IReservationRepository reservationRepository, IPublishEndpoint bus)
    {
        _cleaningTaskRepository = cleaningTaskRepository;
        _cleanerRepository = cleanerRepository;
        _reservationRepository = reservationRepository;
        _bus = bus;
    }

    public async Task<CleaningTask?> GetTaskByIdAsync(Guid id)
    {
        return await _cleaningTaskRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<CleaningTask>> GetTasksByCleanerIdAsync(Guid cleanerId)
    {
        return await _cleaningTaskRepository.GetByCleanerIdAsync(cleanerId);
    }

    public async Task<Guid> CreateOnDemandAsync(Guid reservationId, DateOnly realisationDate, int roomNumber)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation is null)
        {
            throw new ReservationNotFoundException(reservationId);
        }
        
        if (realisationDate < DateOnly.FromDateTime(reservation.StartDate) 
            || realisationDate >  DateOnly.FromDateTime(reservation.EndDate))
        {
            throw new InvalidRealisationDateException(reservation.StartDate, reservation.EndDate);
        }

        var cleaner = await _cleanerRepository.getOneWithLeastTasksAsync();
        if (cleaner is null)
        {
            throw new NoCleanersAvailableException();
        }
        
        var task = new CleaningTask
        {
            Id = Guid.NewGuid(),
            Reservation = reservation,
            RealisationDate = realisationDate.ToDateTime(new TimeOnly(12,0)),
            RoomNumber = roomNumber,
            Status = TaskStatus.Pending,
            CleaningType = CleaningType.OnDemand,
            Cleaner = cleaner
        };
        
        await _cleaningTaskRepository.AddAsync(task);
        var taskEvent = new OnDemandCleaningTaskCreatedEvent
        {
            TaskId = task.Id,
            ReservationId = task.Reservation.Id
        };
        await _bus.Publish(taskEvent);
        return task.Id;
    }

    public async Task UpdateStatusAsync(Guid cleaningTaskId, Guid cleanerId, TaskStatus status)
    {
        var cleaningTask = await _cleaningTaskRepository.GetByIdAsync(cleaningTaskId);
        if (cleaningTask is null)
        {
            throw new CleaningTaskNotFoundException(cleaningTaskId);
        }
        if (cleaningTask.Cleaner is null)
        {
            throw new CleanerNotAssignedToTaskException(cleaningTaskId);
        }
        if (cleaningTask.Cleaner.Id != cleanerId)
        {
            throw new TokenException($"Cleaner with id {cleanerId} is not assigned to this task");
        }
        cleaningTask.Status = status;
        await _cleaningTaskRepository.UpdateAsync(cleaningTask);
    }

    public async Task CreateCleaningTasksForReservation(ReservationCreatedEvent reservationEvent)
    { 
        var reservation = await _reservationRepository.GetByIdAsync(reservationEvent.ReservationId);
        if (reservation is not null)
        {
            throw new ReservationAlreadyExistsException(reservationEvent.ReservationId);
        }
        var newReservation = new Reservation
        {
            Id = reservationEvent.ReservationId,
            StartDate = reservationEvent.StartDate,
            EndDate = reservationEvent.EndDate,
            RoomNumbers = reservationEvent.Rooms.Select(x => x.Id).ToList()
        };
        await _reservationRepository.AddAsync(newReservation);
        
        var cleaningCreations = new List<Task<List<CleaningTask>>>();
        foreach (var room in reservationEvent.Rooms)
        {
            if (room.Type == RoomType.Basic)
            {
                 cleaningCreations.Add(CreateCleaningTasksForBasicRoom(room.Id, newReservation));
            }
            else if (room.Type == RoomType.Economy)
            {
                cleaningCreations.Add(CreateCleaningTasksForEconomyRoom(room.Id, newReservation));
            }
            else if (room.Type == RoomType.Premium)
            {
                cleaningCreations.Add(CreateCleaningTasksForPremiumRoom(room.Id, newReservation));
            }
        }
        var cleaningTasks = (await Task.WhenAll(cleaningCreations))
            .SelectMany(x => x)
            .ToList();
        var cleanersWithTaskCount = await _cleanerRepository.GetCleanersOrderedByTaskCountAsync();
        var assignedTasks = AssignTasksToCleaners(cleaningTasks, cleanersWithTaskCount.ToList());
        await _cleaningTaskRepository.AddRangeAsync(assignedTasks);
    }

    public async Task RemoveCleaningTasksForReservation(ReservationCanceledEvent reservationCanceledEvent)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationCanceledEvent.ReservationId);
        if (reservation is null)
        {
            throw new ReservationNotFoundException(reservationCanceledEvent.ReservationId);
        }
        var tasks = await _cleaningTaskRepository.GetByReservationIdAsync(reservationCanceledEvent.ReservationId);
        await _cleaningTaskRepository.DeleteRangeAsync(tasks);
        await _reservationRepository.DeleteAsync(reservation);
    }

    public static IEnumerable<CleaningTask> AssignTasksToCleaners(List<CleaningTask> cleaningTasks, 
        List<CleanerWithTaskCountDto> cleanersWithTaskCount)
    {
        foreach (var task in cleaningTasks)
        {
            var cleaner = cleanersWithTaskCount
                .OrderBy(x => x.TaskCount)
                .First();
            cleaner.TaskCount++;
            task.Cleaner = cleaner.Cleaner;
        }
        return cleaningTasks;
    }
    
    private Task<List<CleaningTask>> CreateCleaningTasksForBasicRoom(int roomNumber, Reservation reservation)
    {
        var defaultCleaner = new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = new List<CleaningTask>()
        };
        var cleaningTasks = new List<CleaningTask>();
        var startDate = reservation.StartDate;
        var endDate = reservation.EndDate;
        
        for (var date = startDate; date <= endDate; date = date.AddDays(2))
        {
            var task = new CleaningTask
            {
                Id = Guid.NewGuid(),
                Reservation = reservation,
                RealisationDate = date,
                RoomNumber = roomNumber,
                Status = TaskStatus.Pending,
                CleaningType = CleaningType.Cyclic,
                Cleaner = defaultCleaner
            };
            cleaningTasks.Add(task);
        }

        if (cleaningTasks.Any(x => x.RealisationDate == endDate) is false)
        {
            cleaningTasks.Add(new CleaningTask
            {
                Id = Guid.NewGuid(),
                Reservation = reservation,
                RealisationDate = endDate,
                RoomNumber = roomNumber,
                Status = TaskStatus.Pending,
                CleaningType = CleaningType.Cyclic,
                Cleaner = defaultCleaner
            });
        }
        return Task.FromResult(cleaningTasks);
    }

    private Task<List<CleaningTask>> CreateCleaningTasksForEconomyRoom(int roomNumber, Reservation reservation)
    {
        var defaultCleaner = new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = new List<CleaningTask>()
        };
        var startDate = reservation.StartDate;
        var endDate = reservation.EndDate;

        var cleaningTasks = new List<CleaningTask>()
        {
            new CleaningTask
            {
                Id = Guid.NewGuid(),
                Reservation = reservation,
                RealisationDate = startDate,
                RoomNumber = roomNumber,
                Status = TaskStatus.Pending,
                CleaningType = CleaningType.Cyclic,
                Cleaner = defaultCleaner
            },
            new CleaningTask
            {
                Id = Guid.NewGuid(),
                Reservation = reservation,
                RealisationDate = endDate,
                RoomNumber = roomNumber,
                Status = TaskStatus.Pending,
                CleaningType = CleaningType.Cyclic,
                Cleaner = defaultCleaner
            }
        };
        return Task.FromResult(cleaningTasks);
    }

    private Task<List<CleaningTask>> CreateCleaningTasksForPremiumRoom(int roomNumber, Reservation reservation)
    {
        var defaultCleaner = new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = new List<CleaningTask>()
        };
        var cleaningTasks = new List<CleaningTask>();
        var startDate = reservation.StartDate;
        var endDate = reservation.EndDate;
        
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var task = new CleaningTask
            {
                Id = Guid.NewGuid(),
                Reservation = reservation,
                RealisationDate = date,
                RoomNumber = roomNumber,
                Status = TaskStatus.Pending,
                CleaningType = CleaningType.Cyclic,
                Cleaner = defaultCleaner
            };
            cleaningTasks.Add(task);
        }
        return Task.FromResult(cleaningTasks);
    }
}