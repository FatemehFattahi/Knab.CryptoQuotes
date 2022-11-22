using Knab.CryptoQuote.Application.Services;
using Knab.CryptoQuote.Domain;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Wrap;

namespace Knab.CryptoQuote.Infrastructure.Services;

public class ExchangeRateComposer : IExchangeRateService
{
    private IExchangeRateService _currentlyUsingExchangeRateService;
    private readonly IEnumerable<IExchangeRateService> _exchangeRateServices;

    public ExchangeRateComposer(IServiceScopeFactory scopeFactory)
    {
        _exchangeRateServices = scopeFactory.CreateAsyncScope().ServiceProvider
            .GetRequiredService<IEnumerable<IExchangeRateService>>();
        
        _currentlyUsingExchangeRateService = _exchangeRateServices.First();
    }
    
    public Task<CryptoCurrency> GetQuotesByCryptoAsync(string cryptoCurrencyCode, CancellationToken cancellationToken)
    {
        return GetResiliencyPolicy(cryptoCurrencyCode)
            .ExecuteAsync(() => _currentlyUsingExchangeRateService.GetQuotesByCryptoAsync(cryptoCurrencyCode, cancellationToken));
    }

    private void ChangeExchangeRateService()
    {
        _currentlyUsingExchangeRateService = _exchangeRateServices
            .First(service => service.GetType() != _currentlyUsingExchangeRateService.GetType());
    }

    private AsyncPolicyWrap<CryptoCurrency> GetResiliencyPolicy(string cryptoCurrencyCode)
    {
        var retryPolicy = Policy<CryptoCurrency>
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))!;

        var fallbackPolicy = Policy<CryptoCurrency>
            .Handle<HttpRequestException>()
            .FallbackAsync(async cancellationToken =>
            {
                ChangeExchangeRateService();
                return await GetQuotesByCryptoAsync(cryptoCurrencyCode, cancellationToken);
            });
        
        var circuitBreakerPolicy = Policy<CryptoCurrency>
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(6, TimeSpan.FromSeconds(45))!;

        return Policy.WrapAsync(retryPolicy, fallbackPolicy, circuitBreakerPolicy);
    }
}