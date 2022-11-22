using System.Net.Http.Json;
using Knab.CryptoQuote.Application.Services;
using Knab.CryptoQuote.Domain;
using Knab.CryptoQuote.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Knab.CryptoQuote.Infrastructure.Services.ExchangeRates;

public sealed class ExchangeRatesService : IExchangeRateService
{
    private readonly HttpClient _exchangeRatesClient;
    private readonly ExchangeApiOptions _exchangeOptions;

    public ExchangeRatesService(IHttpClientFactory httpClientFactory,
        IOptions<ExchangeApiOptions> exchangeOptions)
    {
        _exchangeRatesClient = httpClientFactory.CreateClient(nameof(ExchangeApiOptions.ExchangeRates));
        _exchangeOptions = exchangeOptions.Value;
    }
    
    public async Task<CryptoCurrency> GetQuotesByCryptoAsync(string cryptoCurrencyCode, CancellationToken cancellationToken)
    {
        var currencyCodes = string.Join(',', Enum.GetNames(typeof(CurrencyCode)));
        var endpoint = string.Format(_exchangeOptions.ExchangeRates.RatesEndpoint, currencyCodes, cryptoCurrencyCode);
        
        var response = await _exchangeRatesClient.GetFromJsonAsync<ExchangeRateApiResponse>(endpoint, cancellationToken);

        var quotes = response!.Rates
            .Select(rate => new Quote(Enum.Parse<CurrencyCode>(rate.Key), rate.Value));
        
        return new(response!.BaseCurrency, quotes);
    }
}