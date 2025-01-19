using HotelUp.Cleaning.Shared.Exceptions;

namespace HotelUp.Cleaning.Services.Services.Exceptions;

public class ReservationNotFoundException : NotFoundException
{
    public ReservationNotFoundException(Guid id) 
        : base($"Reservation with id {id} doesn't exist or doesn't have any assigned cleaning tasks.")
    {
    }
}