using HotelUp.Cleaning.Shared.Exceptions;

namespace HotelUp.Cleaning.Services.Services.Exceptions;

public class CleaningTaskNotFoundException : NotFoundException
{
    public CleaningTaskNotFoundException(Guid id) : base($"Cleaning task with id {id} was not found.")
    {
    }
}