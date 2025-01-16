using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelUp.Cleaning.API.Controllers;

[ApiController]
[Route("api/cleaning/example")]
public class ExampleController : ControllerBase
{
    [Authorize]
    [HttpGet("logged-in-user")]
    public IActionResult GetLoggedInUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        return Ok($"Hello {email}!");
    }
}