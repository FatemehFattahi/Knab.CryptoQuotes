using System.Text.Json.Nodes;
using Knab.CryptoQuote.Application.Services;
using Knab.CryptoQuote.Domain;
using Knab.CryptoQuote.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Knab.CryptoQuote.Infrastructure.Services.CoinMarketCap;

public sealed class CoinMarketCapService : IExchangeRateService
{
    private readonly HttpClient _coinMarketCapClient;
    private readonly ExchangeApiOptions _exchangeOptions;
    private readonly ILogger<CoinMarketCapService> _logger;

    public CoinMarketCapService(HttpClient coinMarketCapClient,
        IOptions<ExchangeApiOptions> exchangeOptions,
        ILogger<CoinMarketCapService> logger)
    {
        _coinMarketCapClient = coinMarketCapClient;
        _exchangeOptions = exchangeOptions.Value;
        _logger = logger;
    }

    public async Task<CryptoCurrency> GetQuotesByCryptoAsync(string cryptoCurrencyCode, CancellationToken cancellationToken)
    {
        cryptoCurrencyCode = cryptoCurrencyCode.ToUpperInvariant();
        
        var prices = Enum.GetNames(typeof(CurrencyCode))
            .Select(async currency =>
            {
                try
                {
                    var price = await GetPriceAsync(cryptoCurrencyCode, currency, cancellationToken);
                    return (currency, price, IsSuccesful: true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during fetching rates for {Currency}", currency);
                    return (string.Empty, 0, IsSuccesful: false);
                }
            });

        var quotes = (await Task.WhenAll(prices))
            .Where(rate => rate.IsSuccesful)
            .Select(rate => new Quote(Enum.Parse<CurrencyCode>(rate.currency), rate.price));
        
        return new(cryptoCurrencyCode, quotes);
    }

    private async ValueTask<decimal> GetPriceAsync(string cryptoCurrencyCode,
        string currencyCode,
        CancellationToken cancellationToken)
    {
        var endpoint = string.Format(_exchangeOptions.CoinMarketCap.RatesEndpoint, cryptoCurrencyCode, currencyCode);
        var jsonResponse = await _coinMarketCapClient.GetStringAsync(endpoint, cancellationToken);

        return JsonNode.Parse(jsonResponse)!
            ["data"]!
            [cryptoCurrencyCode.ToUpperInvariant()]!
            [0]!
            ["quote"]!
            [currencyCode.ToUpperInvariant()]!
            ["price"]!
            .GetValue<decimal>();
    }
}