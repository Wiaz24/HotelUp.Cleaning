using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Cleaning.Persistence.Repositories;
using HotelUp.Cleaning.Services.Events.External.DTOs;
using HotelUp.Employee.Services.Events;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace HotelUp.Cleaning.Tests.Integration.EventHandlers;

[Collection(nameof(EmployeeCreatedEventHandlerTests))]
public class EmployeeCreatedEventHandlerTests : IntegrationTestsBase
{
    private readonly IBus _bus;
    
    public EmployeeCreatedEventHandlerTests(TestWebAppFactory factory, ITestOutputHelper testOutputHelper) 
        : base(factory, testOutputHelper)
    {
        _bus = ServiceProvider.GetRequiredService<IBus>();
    }
    
    private async Task<Cleaner?> GetCleanerAsync(Guid employeeId)
    {
        using var scope = ServiceProvider.CreateScope();
        var cookService = scope.ServiceProvider.GetRequiredService<ICleanerRepository>();
        return await cookService.GetAsync(employeeId);
    }
    
    [Fact]
    public async Task HandleAsync_WhenEmployeeIsCook_ShouldCreateCook()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employeeCreatedEvent = new EmployeeCreatedEvent
        {
            Id = employeeId,
            Role = EmployeeType.Cleaner
        };
        
        // Act
        await _bus.Publish(employeeCreatedEvent);
        await Task.Delay(500);
        
        
        // Assert
        var cleaner = await GetCleanerAsync(employeeId);
        cleaner.ShouldNotBeNull();
        cleaner.Id.ShouldBe(employeeId);
    }
}