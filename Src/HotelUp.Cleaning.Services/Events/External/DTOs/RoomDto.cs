namespace HotelUp.Cleaning.Services.Events.External.DTOs;

public record RoomDto
{
    public int Id { get; init; }
    public int Capacity { get; init; }
    public int Floor { get; init; }
    public bool WithSpecialNeeds { get; init; }
    public RoomType Type { get; init; }
    public required string ImageUrl { get; init; }
}