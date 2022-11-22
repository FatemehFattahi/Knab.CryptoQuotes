using System.Text.Json.Serialization;
using Knab.CryptoQuote.Api.Extensions;
using Knab.CryptoQuote.Application.Services;
using Knab.CryptoQuote.Domain;
using Knab.CryptoQuote.Infrastructure;
using Microsoft.AspNetCore.Http.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args).UseSerilog();

builder.Services.Configure<JsonOptions>(jsonOptions =>
    jsonOptions.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();
app.UseGlobalExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/quotes/{cryptoCode:required}", async (string cryptoCode,
        IExchangeRateComposer exchangeRateComposer, CancellationToken cancellationToken) =>
    {
        var cryptoRate = await exchangeRateComposer.GetQuotesByCryptoAsync(cryptoCode, cancellationToken);

        return TypedResults.Ok(cryptoRate);
    })
    .WithName("GetCryptoQuotes")
    .WithOpenApi()
    .Produces<CryptoCurrency>();

await app.RunAsync(app.Lifetime.ApplicationStopped);

namespace Knab.CryptoQuote
{
    public abstract class Program
    {
    }
}