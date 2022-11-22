using Knab.CryptoQuote.Application.Services;
using Knab.CryptoQuote.Infrastructure.Options;
using Knab.CryptoQuote.Infrastructure.Services;
using Knab.CryptoQuote.Infrastructure.Services.CoinMarketCap;
using Knab.CryptoQuote.Infrastructure.Services.ExchangeRates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Knab.CryptoQuote.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCoinMarketCap();
        services.AddExchangeRates();
        services.AddSingleton<IExchangeRateComposer, ExchangeRateComposer>();
        
        services.AddOptions(configuration);
        
        return services;
    }

    private static void AddCoinMarketCap(this IServiceCollection services)
    {
        services.AddScoped<IExchangeRateService, CoinMarketCapService>();

        services.AddHttpClient(nameof(ExchangeApiOptions.CoinMarketCap), (sp, httpClient) =>
        {
            var exchangeApiOptions = sp.GetRequiredService<IOptions<ExchangeApiOptions>>().Value;

            httpClient.BaseAddress = new(exchangeApiOptions.CoinMarketCap.Url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(exchangeApiOptions.CoinMarketCap.ApiKeyHeaderName,
                exchangeApiOptions.CoinMarketCap.ApiKey);
        });
    }
    
    private static void AddExchangeRates(this IServiceCollection services)
    {
        services.AddScoped<IExchangeRateService, ExchangeRatesService>();

        services.AddHttpClient(nameof(ExchangeApiOptions.ExchangeRates), (sp, httpClient) =>
        {
            var exchangeApiOptions = sp.GetRequiredService<IOptions<ExchangeApiOptions>>().Value;

            httpClient.BaseAddress = new(exchangeApiOptions.ExchangeRates.Url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(exchangeApiOptions.ExchangeRates.ApiKeyHeaderName,
                exchangeApiOptions.ExchangeRates.ApiKey);
        });
    }
    
    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ExchangeApiOptions>()
            .Bind(configuration.GetSection(nameof(ExchangeApiOptions)), options => 
                options.ErrorOnUnknownConfiguration = true)
            .Validate(exchangeApiOptions => !string.IsNullOrWhiteSpace(exchangeApiOptions.CoinMarketCap.Url) &&
                                            !string.IsNullOrWhiteSpace(exchangeApiOptions.CoinMarketCap.ApiKey) &&
                                            !string.IsNullOrWhiteSpace(exchangeApiOptions.CoinMarketCap.RatesEndpoint) &&
                                            !string.IsNullOrWhiteSpace(exchangeApiOptions.ExchangeRates.Url) &&
                                            !string.IsNullOrWhiteSpace(exchangeApiOptions.ExchangeRates.ApiKey) &&
                                            !string.IsNullOrWhiteSpace(exchangeApiOptions.ExchangeRates.RatesEndpoint))
            .ValidateOnStart();
    }
}