using System.ComponentModel;
using System.Security.Claims;
using HotelUp.Cleaning.API.DTOs;
using HotelUp.Cleaning.Persistence.Entities;
using HotelUp.Cleaning.Services.Services;
using HotelUp.Cleaning.Shared.Auth;
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
    private Guid LoggedInUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) 
        is { } id ? new Guid(id) : throw new TokenException("No user id found in access token.");
    private readonly ICleaningTaskService _cleaningTaskService;

    public CleaningTaskController(ICleaningTaskService cleaningTaskService)
    {
        _cleaningTaskService = cleaningTaskService;
    }

    [HttpGet("{id:guid}")]
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
    
    [HttpGet]
    [Authorize(Policy = PoliciesNames.CanManageCleaningTasks)]
    [SwaggerOperation("Get all cleaning tasks for logged in cleaner")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<CleaningTask>>> GetAll()
    {
        var result = await _cleaningTaskService.GetTasksByCleanerIdAsync(LoggedInUserId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [SwaggerOperation("Create new cleaning task on demand as a client or receptionist")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCleaningTaskOnDemand([FromBody] CreateCleaningTaskDto dto)
    {
        var result = await _cleaningTaskService.CreateOnDemandAsync(
            dto.ReservationId, 
            dto.RealisationDate, 
            dto.RoomNumber);
        return CreatedAtAction(nameof(GetById), new { id = result}, result);
    }
    
    [Authorize(Policy = PoliciesNames.CanManageCleaningTasks)]
    [HttpPut("{id:guid}")]
    [SwaggerOperation("Update cleaning task status as a cleaner")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTaskStatus([FromRoute] Guid id, [FromQuery] UpdateCleaningTaskStatusDto dto)
    {
        await _cleaningTaskService.UpdateStatusAsync(id, LoggedInUserId, dto.Status);
        return Ok();
    }
    
}