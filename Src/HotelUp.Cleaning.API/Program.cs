using System.Text.Json.Serialization;
using HotelUp.Cleaning.API.Cors;
using HotelUp.Cleaning.API.Swagger;
using HotelUp.Cleaning.Persistence;
using HotelUp.Cleaning.Services;
using HotelUp.Cleaning.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddShared();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddCorsForFrontend(builder.Configuration);
builder.Services.AddPersistenceLayer();
builder.Services.AddServiceLayer();

var app = builder.Build();

app.UseShared();
app.UseCustomSwagger();
app.UseCorsForFrontend();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/api/cleaning/swagger/index.html"))
    .Produces(200)
    .ExcludeFromDescription();
app.Run();

public interface IApiMarker
{
}