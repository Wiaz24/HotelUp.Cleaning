using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using HotelUp.Cleaning.API.DTOs;
using HotelUp.Cleaning.Persistence.EF;
using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Customer.Tests.Integration.Utils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace HotelUp.Cleaning.Tests.Integration.Controllers;

[Collection(nameof(CleaningTaskControllerTests))]
public class CleaningTaskControllerTests : IntegrationTestsBase
{
    private readonly ITestOutputHelper _testOutputHelper;
    private const string Prefix = "api/cleaning/cleaning-task";
    public CleaningTaskControllerTests(TestWebAppFactory factory, ITestOutputHelper testOutputHelper) 
        : base(factory)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    private HttpClient CreateHttpClientWithToken(Guid clientId)
    {
        var httpClient = Factory.CreateClient();
        var token = MockJwtTokens.GenerateJwtToken(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, clientId.ToString()),
            new Claim(ClaimTypes.Role, "Clients")
        });
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return httpClient;
    }

    private async Task<Reservation> CreateSampleReservation()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomNumbers = [1, 3],
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(2)
        };
        dbContext.Reservations.Add(reservation);
        await dbContext.SaveChangesAsync();
        return reservation;
    }
    
    private async Task<Cleaner> CreateSampleCleaner()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var cleaner = new Cleaner
        {
            Id = Guid.NewGuid(),
            CleaningTasks = []
        };
        dbContext.Cleaners.Add(cleaner);
        await dbContext.SaveChangesAsync();
        return cleaner;
    }
    
    [Fact]
    public async Task CreateCleaningTaskOnDemand_WhenReservationExists_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var reservation = await CreateSampleReservation();
        var httpClient = CreateHttpClientWithToken(reservation.Id);
        await CreateSampleCleaner();
        
        var dto = new CreateCleaningTaskDto
        {
            ReservationId = reservation.Id,
            RoomNumber = reservation.RoomNumbers.First(),
            RealisationDate = DateOnly.FromDateTime(reservation.EndDate)
        };
        
        // Act
        var response = await httpClient.PostAsJsonAsync($"{Prefix}", dto);
        var content = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(content);
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
    
}