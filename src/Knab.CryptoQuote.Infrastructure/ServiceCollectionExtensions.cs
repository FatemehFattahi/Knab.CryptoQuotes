using Knab.CryptoQuote.Application.Services;
using Knab.CryptoQuote.Infrastructure.Options;
using Knab.CryptoQuote.Infrastructure.Services.CoinMarketCap;
using Knab.CryptoQuote.Infrastructure.Services.ExchangeRates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Knab.CryptoQuote.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCoinMarketCapClient();
        services.AddExchangeRatesClient();
        
        services.AddOptions(configuration);
        
        return services;
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ExchangeApiOptions>()
            .Bind(configuration.GetSection(nameof(ExchangeApiOptions)), options => 
                options.ErrorOnUnknownConfiguration = true)
            .Validate(exchangeApiOptions => !string.IsNullOrWhiteSpace(exchangeApiOptions.CoinMarketCap.Url) &&
                                            !string.IsNullOrWhiteSpace(exchangeApiOptions.CoinMarketCap.ApiKey))
            .ValidateOnStart();
    }

    private static void AddCoinMarketCapClient(this IServiceCollection services)
    {
        services.AddHttpClient<IExchangeRateService, CoinMarketCapService>("", (sp, httpClient) =>
        {
            var exchangeApiOptions = sp.GetRequiredService<IOptions<ExchangeApiOptions>>().Value;
            
            httpClient.BaseAddress = new(exchangeApiOptions.CoinMarketCap.Url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-CMC_PRO_API_KEY", exchangeApiOptions.CoinMarketCap.ApiKey);
        }).AddPolicyHandler(GetRetryPolicy());
    }
    
    private static void AddExchangeRatesClient(this IServiceCollection services)
    {
        services.AddHttpClient<IExchangeRateService, ExchangeRatesService>("", (sp, httpClient) =>
        {
            var exchangeApiOptions = sp.GetRequiredService<IOptions<ExchangeApiOptions>>().Value;
            
            httpClient.BaseAddress = new(exchangeApiOptions.ExchangeRates.Url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", exchangeApiOptions.ExchangeRates.ApiKey);
        }).AddPolicyHandler(GetRetryPolicy());
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}