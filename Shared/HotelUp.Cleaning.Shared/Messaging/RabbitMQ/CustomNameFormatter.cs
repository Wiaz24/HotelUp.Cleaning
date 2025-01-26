using MassTransit;

namespace HotelUp.Cleaning.Shared.Messaging.RabbitMQ;

public class CustomNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        var namespaceParts = typeof(T).Namespace?.Split('.');
        if (namespaceParts == null)
        {
            return typeof(T).Name;
        }
        return $"{namespaceParts[0]}.{namespaceParts[1]}:{typeof(T).Name}";
    }
}