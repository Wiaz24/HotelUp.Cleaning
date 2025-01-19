using HotelUp.Cleaning.Shared.Exceptions;

namespace HotelUp.Cleaning.Services.Services.Exceptions;

public class InvalidRealisationDateException : BusinessRuleException
{
    public InvalidRealisationDateException() : base("Realisation date cannot be in the past.")
    {
    }
}