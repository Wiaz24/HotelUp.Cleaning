using System.ComponentModel;
using System.Security.Claims;
using HotelUp.Cleaning.API.DTOs;
using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Cleaning.Services.Services;
using HotelUp.Cleaning.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelUp.Cleaning.API.Controllers;

[ApiController]
[Route("api/cleaning/cleaning-task")]
[ProducesErrorResponseType(typeof(ErrorResponse))]
public class CleaningTaskController : ControllerBase
{
    private readonly ICleaningTaskService _cleaningTaskService;

    public CleaningTaskController(ICleaningTaskService cleaningTaskService)
    {
        _cleaningTaskService = cleaningTaskService;
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Get task by id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CleaningTask>> GetById([FromRoute] Guid id)
    {
        var result = await _cleaningTaskService.GetTaskByIdAsync(id);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [SwaggerOperation("Create new cleaning task on demand as a client or receptionist")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateTask([FromBody] CreateCleaningTaskDto dto)
    {
        var result = await _cleaningTaskService.CreateOnDemandAsync(
            dto.ReservationId, 
            dto.RealisationDate, 
            dto.RoomNumber);
        return CreatedAtAction(nameof(GetById), new { id = result}, result);
    }
}