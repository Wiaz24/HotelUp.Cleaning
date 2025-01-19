using Swashbuckle.AspNetCore.Annotations;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;
namespace HotelUp.Cleaning.API.DTOs;

public record UpdateCleaningTaskStatusDto
{
    [SwaggerParameter("Cleaning task status")]
    public required TaskStatus Status { get; init; }
}