using Knab.CryptoQuote.Domain;

namespace Knab.CryptoQuote.Application.Services;

public interface IExchangeRateService
{
    Task<CryptoCurrency> GetQuotesByCryptoAsync(string cryptoCurrencyCode, CancellationToken cancellationToken);
}