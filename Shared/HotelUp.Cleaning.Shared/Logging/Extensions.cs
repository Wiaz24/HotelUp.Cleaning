// #define SendLogsToSeq

using HotelUp.Cleaning.Shared.Logging.Seq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace HotelUp.Cleaning.Shared.Logging;

internal static class Extensions
{
    internal static WebApplicationBuilder AddCustomLogging(this WebApplicationBuilder builder)
    {
        builder.Services.AddSeqLogging();

        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.Console();
            var seqOptions = context.Configuration.GetSection("Seq").Get<SeqOptions>()
                             ?? throw new NullReferenceException("Seq configuration is missing.");
            loggerConfiguration.WriteTo.Seq(seqOptions.ServerUrl);
        });
        return builder;
    }
}