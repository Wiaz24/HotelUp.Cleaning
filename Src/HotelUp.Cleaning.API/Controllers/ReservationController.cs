using HotelUp.Cleaning.Services.Events.External.DTOs;
using HotelUp.Cleaning.Shared.Exceptions;
using HotelUp.Customer.Application.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelUp.Cleaning.API.Controllers;

[ApiController]
[Route("api/cleaning/reservation")]
[ProducesErrorResponseType(typeof(ErrorResponse))]
public class ReservationController : ControllerBase
{
    private readonly IPublishEndpoint _bus;

    public ReservationController(IPublishEndpoint bus)
    {
        _bus = bus;
    }


    [Obsolete]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("ONLY FOR TESTING! Create example reservation. In production this will be done by integration event.")]
    public async Task<IActionResult> TestCleanerCreatedEvent()
    {
        var roomDto = new RoomDto
        {
            Capacity = 1,
            Floor = 1,
            Id = 1,
            ImageUrl = "https://example.com/image.png",
            Type = RoomType.Basic,
            WithSpecialNeeds = false
        };
        var reservationCreatedEvent = new ReservationCreatedEvent
        {
            ReservationId = Guid.NewGuid(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            Rooms = [roomDto]
        };
        await _bus.Publish(reservationCreatedEvent);
        return Created("", reservationCreatedEvent.ReservationId);
    }
}