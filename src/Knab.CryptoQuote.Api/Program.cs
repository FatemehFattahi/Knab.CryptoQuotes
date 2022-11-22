using System.Text.Json.Serialization;
using Knab.CryptoQuote.Api.Extensions;
using Knab.CryptoQuote.Domain;
using Knab.CryptoQuote.Infrastructure;
using Knab.CryptoQuote.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args).UseSerilog();

builder.Services.Configure<JsonOptions>(jsonOptions =>
    jsonOptions.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/quotes/{cryptoCode:required}", async (string cryptoCode, 
    ExchangeRateComposer exchangeRateComposer, CancellationToken cancellationToken) =>
{
    var cryptoRate = await exchangeRateComposer.GetQuotesByCryptoAsync(cryptoCode, cancellationToken);
    return TypedResults.Ok(cryptoRate);
})
.WithName("GetQuotesForSpecificCryptoCurrency")
.WithOpenApi()
.Produces<CryptoCurrency>();

await app.RunAsync(app.Lifetime.ApplicationStopped);