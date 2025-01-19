using HotelUp.Cleaning.Shared.Exceptions;

namespace HotelUp.Cleaning.Services.Services.Exceptions;

public class NoCleanersAvailableException : BusinessRuleException       
{
    public NoCleanersAvailableException() : base("No cleaners are available.")
    {
    }
}