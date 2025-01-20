using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Cleaning.Persistence.Repositories;
using HotelUp.Cleaning.Services.Services.Exceptions;
using NSubstitute;
using Shouldly;

namespace HotelUp.Cleaning.Tests.Unit.Services.CleaningTaskServiceTests;

public class CreateOnDemandTests
{
    [Fact]
    public async Task CreateOnDemandAsync_WhenReservationExistsAndCleanerIsAvailable_CreatesTask()
    {
        // Arrange
        var cleaningTaskRepository = Substitute.For<ICleaningTaskRepository>();
        var cleanerRepository = Substitute.For<ICleanerRepository>();
        var reservationRepository = Substitute.For<IReservationRepository>();
        var today = new DateTime(2025, 1, 1);
        
        reservationRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(new Reservation
        {
            Id = Guid.NewGuid(),
            RoomNumbers = [1],
            StartDate = today,
            EndDate = today.AddDays(2)
        });
        cleanerRepository.getOneWithLeastTasksAsync().Returns(new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = []
        });
        
        var service = new Cleaning.Services.Services.CleaningTaskService(cleaningTaskRepository, cleanerRepository, reservationRepository);
        var reservationId = Guid.NewGuid();
        var realisationDate = today.AddDays(1);
        var roomNumber = 1;

        // Act
        var result = await service.CreateOnDemandAsync(reservationId, realisationDate, roomNumber);

        // Assert
        result.ShouldNotBe(Guid.Empty);
    }
    
    [Fact]
    public async Task CreateOnDemandAsync_WhenRealisationDateIsNotInReservationPeriod_ThrowsInvalidRealisationDateException()
    {
        // Arrange
        var cleaningTaskRepository = Substitute.For<ICleaningTaskRepository>();
        var cleanerRepository = Substitute.For<ICleanerRepository>();
        var reservationRepository = Substitute.For<IReservationRepository>();
        var today = new DateTime(2025, 1, 1);
        
        reservationRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(new Reservation
        {
            Id = Guid.NewGuid(),
            RoomNumbers = [1],
            StartDate = today,
            EndDate = today.AddDays(2)
        });
        cleanerRepository.getOneWithLeastTasksAsync().Returns(new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = []
        });
        
        
        var service = new Cleaning.Services.Services.CleaningTaskService(cleaningTaskRepository, cleanerRepository, reservationRepository);
        var reservationId = Guid.NewGuid();
        var realisationDate = today.AddDays(3);
        var roomNumber = 1;

        // Act
        var exception = await Record.ExceptionAsync(async () => 
            await service.CreateOnDemandAsync(reservationId, realisationDate, roomNumber));

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
        var reservationRepository = Substitute.For<IReservationRepository>();
        var today = new DateTime(2025, 1, 1);
        
        reservationRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Reservation?)null);
        cleanerRepository.getOneWithLeastTasksAsync().Returns(new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = []
        });
        
        
        var service = new Cleaning.Services.Services.CleaningTaskService(cleaningTaskRepository, cleanerRepository, reservationRepository);
        var reservationId = Guid.NewGuid();
        var realisationDate = today.AddDays(1);
        var roomNumber = 1;

        // Act
        var exception = await Record.ExceptionAsync(async () => 
            await service.CreateOnDemandAsync(reservationId, realisationDate, roomNumber));

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
        var reservationRepository = Substitute.For<IReservationRepository>();
        var today = new DateTime(2025, 1, 1);
        
        reservationRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(new Reservation
        {
            Id = Guid.NewGuid(),
            RoomNumbers = [1],
            StartDate = today,
            EndDate = today.AddDays(2)
        });
        cleanerRepository.getOneWithLeastTasksAsync().Returns((Cleaner?)null);
        
        var service = new Cleaning.Services.Services.CleaningTaskService(cleaningTaskRepository, cleanerRepository, reservationRepository);
        var reservationId = Guid.NewGuid();
        var realisationDate = today.AddDays(1);
        var roomNumber = 1;

        // Act
        var exception = await Record.ExceptionAsync(async () => 
            await service.CreateOnDemandAsync(reservationId, realisationDate, roomNumber));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<NoCleanersAvailableException>();
    }
}