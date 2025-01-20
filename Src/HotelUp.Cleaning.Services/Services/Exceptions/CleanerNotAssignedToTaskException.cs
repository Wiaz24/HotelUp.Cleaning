using HotelUp.Cleaning.Shared.Exceptions;

namespace HotelUp.Cleaning.Services.Services.Exceptions;

public class CleanerNotAssignedToTaskException : BusinessRuleException
{
    public CleanerNotAssignedToTaskException(Guid taskId) 
        : base($"Cleaner is not assigned to task with id {taskId}")
    {
    }
}