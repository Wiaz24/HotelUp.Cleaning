using System.Text.Json;
using System.Text.RegularExpressions;
using MassTransit.Internals;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HotelUp.Cleaning.Shared.Exceptions;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var type = ex.GetType();
          
            if (type.IsConcreteAndAssignableTo<BusinessRuleException>())
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
            
                var errorCode = ToSnakeCase(ex.GetType().Name.Replace("Exception", ""));
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            else if (type.IsConcreteAndAssignableTo<NotFoundException>())
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";
            
                var errorCode = ToSnakeCase(ex.GetType().Name.Replace("Exception", ""));
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
            else if (type.IsConcreteAndAssignableTo<DatabaseException>())
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.ContentType = "application/json";
                _logger.LogError(ex.Message);
                var errorCode = ToSnakeCase(ex.GetType().Name.Replace("Exception", ""));
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
                
            }
            else if (type.IsConcreteAndAssignableTo<TokenException>())
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else throw;
        }
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var startUnderscores = Regex.Match(input, @"^_+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}