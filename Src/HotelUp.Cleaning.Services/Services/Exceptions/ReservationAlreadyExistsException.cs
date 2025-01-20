using HotelUp.Cleaning.Shared.Exceptions;

namespace HotelUp.Cleaning.Services.Services.Exceptions;

public class ReservationAlreadyExistsException : BusinessRuleException
{
    public ReservationAlreadyExistsException(Guid reservationId)
        : base($"Reservation with id {reservationId} already exists, and have cleaning tasks assigned")
    {
    }
}