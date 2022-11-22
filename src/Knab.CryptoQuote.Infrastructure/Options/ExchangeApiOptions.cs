namespace Knab.CryptoQuote.Infrastructure.Options;

public sealed class ExchangeApiOptions
{
    public required ExchangeApiBase CoinMarketCap { get; init; } = default!;

    public required ExchangeApiBase ExchangeRates { get; init; } = default!;
    
    public class ExchangeApiBase
    {
        public ExchangeApiBase(string url, string apiKey, string ratesEndpoint)
        {
            Url = url;
            ApiKey = apiKey;
            RatesEndpoint = ratesEndpoint;
        }
        
        public required string Url { get; init; }
        
        public required string ApiKey { get; init; }
        
        public required string RatesEndpoint { get; init; }
    }
}