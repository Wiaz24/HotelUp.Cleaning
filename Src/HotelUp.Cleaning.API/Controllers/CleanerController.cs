using System.Security.Claims;
using HotelUp.Cleaning.Services.Services;
using HotelUp.Cleaning.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelUp.Cleaning.API.Controllers;

[ApiController]
[Route("api/cleaning/cleaner")]
[ProducesErrorResponseType(typeof(ErrorResponse))]
public class CleanerController : ControllerBase
{
    private readonly ICleanerService _cleanerService;

    public CleanerController(ICleanerService cleanerService)
    {
        _cleanerService = cleanerService;
    }

    private Guid LoggedInUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) 
        is { } id ? new Guid(id) : throw new TokenException("No user id found in access token.");
    
    [Obsolete]
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("ONLY FOR TESTING! Create new cleaner. In production this will be done by integration event.")]
    public async Task<IActionResult> TestCleanerCreatedEvent()
    {
        var id = LoggedInUserId;
        await _cleanerService.CreateAsync(id);
        return Created("", id);
    }
    
    
}