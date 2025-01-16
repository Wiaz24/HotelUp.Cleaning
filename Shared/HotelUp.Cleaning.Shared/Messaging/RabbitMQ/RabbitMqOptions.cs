namespace HotelUp.Cleaning.Shared.Messaging.RabbitMQ;

public class RabbitMqOptions
{
    public required string Host { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
}