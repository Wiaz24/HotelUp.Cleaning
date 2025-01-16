using HotelUp.Cleaning.API.Cors;
using HotelUp.Cleaning.API.Swagger;
using HotelUp.Cleaning.Persistence;
using HotelUp.Cleaning.Services;
using HotelUp.Cleaning.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddShared();
builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddCorsForFrontend(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddServiceLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);

var app = builder.Build();

app.UseShared();
app.UseCustomSwagger();
app.UseCorsForFrontend();
app.MapControllers();
app.Run();

public interface IApiMarker
{
}