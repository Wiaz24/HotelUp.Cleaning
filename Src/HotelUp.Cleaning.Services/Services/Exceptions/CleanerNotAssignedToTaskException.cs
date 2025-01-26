using HotelUp.Cleaning.Shared.Exceptions;

namespace HotelUp.Cleaning.Services.Services.Exceptions;

public class CleanerNotAssignedToTaskException : BusinessRuleException
{
    public CleanerNotAssignedToTaskException(Guid taskId, Guid cleanerId) 
        : base($"Cleaner with id {cleanerId} is not assigned to task with id {taskId}")
    {
    }
}