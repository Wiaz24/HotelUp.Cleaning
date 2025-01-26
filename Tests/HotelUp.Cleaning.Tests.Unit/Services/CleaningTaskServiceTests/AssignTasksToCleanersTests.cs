using HotelUp.Cleaning.Persistence.Const;
using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Cleaning.Persistence.Repositories;
using HotelUp.Cleaning.Services.Services;
using Shouldly;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;

namespace HotelUp.Cleaning.Tests.Unit.Services.CleaningTaskServiceTests;

public class AssignTasksToCleanersTests
{
    private static readonly DateTime BaseDate = new(2025, 1, 1);
    private static readonly Cleaner DefaultCleaner = new()
    {
        Id = Guid.NewGuid(),
        CleaningTasks = []
    };

    [Fact]
    public void AssignTasksToCleaners_WhenSingleTaskAndSingleCleaner_AssignsTaskToThatCleaner()
    {
        // Arrange
        var cleaner = new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = []
        };

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomNumbers = [101],
            StartDate = BaseDate,
            EndDate = BaseDate.AddDays(1)
        };

        var cleanersWithCount = new List<CleanerWithTaskCountDto>
        {
            new(cleaner, 0)
        };

        var tasks = new List<CleaningTask>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Reservation = reservation,
                RealisationDate = BaseDate,
                RoomNumber = 101,
                CleaningType = CleaningType.Cyclic,
                CleanerId = DefaultCleaner.Id
            }
        };

        // Act
        var result = CleaningTaskService.AssignTasksToCleaners(tasks, cleanersWithCount).ToList();

        // Assert
        result.Count.ShouldBe(1);
        result.First().CleanerId.ShouldBe(cleaner.Id);
        result.First().Status.ShouldBe(TaskStatus.Pending);
    }

    [Fact]
    public void AssignTasksToCleaners_WhenMultipleTasksAndCleaners_AssignsTasksEvenly()
    {
        // Arrange
        var cleaner1 = new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = []
        };

        var cleaner2 = new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = []
        };

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomNumbers = [101, 102],
            StartDate = BaseDate,
            EndDate = BaseDate.AddDays(1)
        };

        var cleanersWithCount = new List<CleanerWithTaskCountDto>
        {
            new(cleaner1, 0),
            new(cleaner2, 0)
        };

        var tasks = new List<CleaningTask>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Reservation = reservation,
                RealisationDate = BaseDate,
                RoomNumber = 101,
                CleaningType = CleaningType.Cyclic,
                CleanerId = DefaultCleaner.Id
            },
            new()
            {
                Id = Guid.NewGuid(),
                Reservation = reservation,
                RealisationDate = BaseDate,
                RoomNumber = 102,
                CleaningType = CleaningType.Cyclic,
                CleanerId = DefaultCleaner.Id
            }
        };

        // Act
        var result = CleaningTaskService.AssignTasksToCleaners(tasks, cleanersWithCount).ToList();

        // Assert
        result.Count.ShouldBe(2);
        result[0].CleanerId.ShouldBe(cleaner1.Id);
        result[1].CleanerId.ShouldBe(cleaner2.Id);
        result.ShouldAllBe(task => task.Status == TaskStatus.Pending);
    }

    [Fact]
    public void AssignTasksToCleaners_WhenCleanerHasExistingTasks_AssignsToLeastBusyCleaner()
    {
        // Arrange
        var cleaner1 = new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = []
        };

        var cleaner2 = new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = []
        };

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomNumbers = [101],
            StartDate = BaseDate,
            EndDate = BaseDate.AddDays(1)
        };

        var cleanersWithCount = new List<CleanerWithTaskCountDto>
        {
            new(cleaner1, 2),
            new(cleaner2, 0)
        };

        var tasks = new List<CleaningTask>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Reservation = reservation,
                RealisationDate = BaseDate,
                RoomNumber = 101,
                CleaningType = CleaningType.OnDemand,
                CleanerId = DefaultCleaner.Id
            }
        };

        // Act
        var result = CleaningTaskService.AssignTasksToCleaners(tasks, cleanersWithCount);

        // Assert
        result.First().CleanerId.ShouldBe(cleaner2.Id);
    }

    [Fact]
    public void AssignTasksToCleaners_WhenNoCleaners_ThrowsInvalidOperationException()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomNumbers = [101],
            StartDate = BaseDate,
            EndDate = BaseDate.AddDays(1)
        };

        var tasks = new List<CleaningTask>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Reservation = reservation,
                RealisationDate = BaseDate,
                RoomNumber = 101,
                CleaningType = CleaningType.OnDemand,
                CleanerId = DefaultCleaner.Id
            }
        };

        var emptyCleanersList = new List<CleanerWithTaskCountDto>();

        // Act & Assert
        Should.Throw<InvalidOperationException>(() =>
            CleaningTaskService.AssignTasksToCleaners(tasks, emptyCleanersList).ToList());
    }

    [Fact]
    public void AssignTasksToCleaners_WhenNoTasks_ReturnsEmptyCollection()
    {
        // Arrange
        var cleaner = new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = []
        };

        var cleanersWithCount = new List<CleanerWithTaskCountDto>
        {
            new(cleaner, 0)
        };

        var emptyTasksList = new List<CleaningTask>();

        // Act
        var result = CleaningTaskService.AssignTasksToCleaners(emptyTasksList, cleanersWithCount);

        // Assert
        result.ShouldBeEmpty();
    }
}