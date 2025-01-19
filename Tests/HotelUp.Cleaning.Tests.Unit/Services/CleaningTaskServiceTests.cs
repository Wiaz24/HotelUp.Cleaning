using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Cleaning.Persistence.Repositories;
using HotelUp.Cleaning.Services.Services;
using HotelUp.Cleaning.Services.Services.Exceptions;
using NSubstitute;
using Shouldly;

namespace HotelUp.Cleaning.Tests.Unit.Services;

public class CleaningTaskServiceTests
{
    [Fact]
    public async Task CreateOnDemandAsync_WhenReservationExistsAndCleanerIsAvailable_CreatesTask()
    {
        // Arrange
        var cleaningTaskRepository = Substitute.For<ICleaningTaskRepository>();
        var cleanerRepository = Substitute.For<ICleanerRepository>();
        var timeProvider = Substitute.For<TimeProvider>();
        var today = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        timeProvider.GetUtcNow().Returns(today);
        
        cleaningTaskRepository.ReservationExistsAsync(Arg.Any<Guid>()).Returns(true);
        cleanerRepository.getOneWithLeastTasksAsync().Returns(new Cleaner(){Id = Guid.NewGuid()});
        
        var service = new CleaningTaskService(cleaningTaskRepository, timeProvider, cleanerRepository);
        var reservationId = Guid.NewGuid();
        var realisationDate = today.AddDays(1);
        var roomNumber = 1;

        // Act
        var result = await service.CreateOnDemandAsync(reservationId, realisationDate.DateTime, roomNumber);

        // Assert
        result.ShouldNotBe(Guid.Empty);
    }
    
    [Fact]
    public async Task CreateOnDemandAsync_WhenRealisationDateIsInThePast_ThrowsInvalidRealisationDateException()
    {
        // Arrange
        var cleaningTaskRepository = Substitute.For<ICleaningTaskRepository>();
        var cleanerRepository = Substitute.For<ICleanerRepository>();
        var timeProvider = Substitute.For<TimeProvider>();
        var today = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        timeProvider.GetUtcNow().Returns(today);
        
        cleaningTaskRepository.ReservationExistsAsync(Arg.Any<Guid>()).Returns(true);
        cleanerRepository.getOneWithLeastTasksAsync().Returns(new Cleaner(){Id = Guid.NewGuid()});
        
        
        var service = new CleaningTaskService(cleaningTaskRepository, timeProvider, cleanerRepository);
        var reservationId = Guid.NewGuid();
        var realisationDate = today.AddDays(-1);
        var roomNumber = 1;

        // Act
        var exception = await Record.ExceptionAsync(async () => 
            await service.CreateOnDemandAsync(reservationId, realisationDate.DateTime, roomNumber));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidRealisationDateException>();
    }
    
    [Fact]
    public async Task CreateOnDemandAsync_WhenReservationDoesNotExist_ThrowsReservationNotFoundException()
    {
        // Arrange
        var cleaningTaskRepository = Substitute.For<ICleaningTaskRepository>();
        var cleanerRepository = Substitute.For<ICleanerRepository>();
        var timeProvider = Substitute.For<TimeProvider>();
        var today = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        timeProvider.GetUtcNow().Returns(today);
        
        cleaningTaskRepository.ReservationExistsAsync(Arg.Any<Guid>()).Returns(false);
        cleanerRepository.getOneWithLeastTasksAsync().Returns(new Cleaner(){Id = Guid.NewGuid()});
        
        
        var service = new CleaningTaskService(cleaningTaskRepository, timeProvider, cleanerRepository);
        var reservationId = Guid.NewGuid();
        var realisationDate = today.AddDays(1);
        var roomNumber = 1;

        // Act
        var exception = await Record.ExceptionAsync(async () => 
            await service.CreateOnDemandAsync(reservationId, realisationDate.DateTime, roomNumber));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ReservationNotFoundException>();
    }
    
    [Fact]
    public async Task CreateOnDemandAsync_WhenNoCleanersAvailable_ThrowsNoCleanersAvailableException()
    {
        // Arrange
        var cleaningTaskRepository = Substitute.For<ICleaningTaskRepository>();
        var cleanerRepository = Substitute.For<ICleanerRepository>();
        var timeProvider = Substitute.For<TimeProvider>();
        var today = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        timeProvider.GetUtcNow().Returns(today);
        
        cleaningTaskRepository.ReservationExistsAsync(Arg.Any<Guid>()).Returns(true);
        cleanerRepository.getOneWithLeastTasksAsync().Returns((Cleaner?)null);
        
        var service = new CleaningTaskService(cleaningTaskRepository, timeProvider, cleanerRepository);
        var reservationId = Guid.NewGuid();
        var realisationDate = today.AddDays(1);
        var roomNumber = 1;

        // Act
        var exception = await Record.ExceptionAsync(async () => 
            await service.CreateOnDemandAsync(reservationId, realisationDate.DateTime, roomNumber));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<NoCleanersAvailableException>();
    }
}