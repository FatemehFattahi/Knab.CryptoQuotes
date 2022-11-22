using Serilog;

namespace Knab.CryptoQuote.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, cfg) =>
        {
            cfg
                .ReadFrom.Services(services)
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext();

            const string consoleOutputTemplate =
                "[{Timestamp:HH:mm:ss} {Level:u3}]:[{SourceContext}] {Message:lj} {NewLine}{Exception}";

            cfg
                .WriteTo
                .Async(sinkCfg => sinkCfg.Console(outputTemplate: consoleOutputTemplate));
        });

        return builder;
    }
}