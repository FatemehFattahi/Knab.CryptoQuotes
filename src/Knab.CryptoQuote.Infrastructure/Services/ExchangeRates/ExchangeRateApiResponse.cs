using System.Text.Json.Serialization;

namespace Knab.CryptoQuote.Infrastructure.Services.ExchangeRates;

#nullable disable

public class ExchangeRateApiResponse
{
    [JsonPropertyName("rates")]
    public Dictionary<string, decimal> Rates { set; get; }
    
    [JsonPropertyName("base")]
    public string BaseCurrency { set; get; }
}