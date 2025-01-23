using HotelUp.Cleaning.Shared.Exceptions;

namespace HotelUp.Cleaning.Services.Services.Exceptions;

public class InvalidRealisationDateException : BusinessRuleException
{
    public InvalidRealisationDateException(DateTime startDate, DateTime endDate) 
        : base($"Realisation date must be between {startDate} and {endDate}")
    {
    }
}