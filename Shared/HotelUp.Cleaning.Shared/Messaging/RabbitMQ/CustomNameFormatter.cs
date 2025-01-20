using MassTransit;

namespace HotelUp.Cleaning.Shared.Messaging.RabbitMQ;

public class CustomNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        return $"HotelUp.Customer:{typeof(T).Name}";
    }
}