using System.Net;
using FluentAssertions;
using Knab.CryptoQuote.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace Knab.CryptoQuote.IntegrationTests;

public class GetCryptoQuotesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetCryptoQuotesTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCryptoQuoteWithValidCode_Should_ReturnQuotes()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var jsonResponse = await client.GetStringAsync("quotes/BTC");
        var currency = JsonConvert.DeserializeObject<CryptoCurrency>(jsonResponse);
        
        // Assert
        currency.Should().NotBeNull();
        currency!.CryptoCurrencyCode.Should().Be("BTC");
        currency.Quotes.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task GetCryptoQuoteWithInvalidCode_Should_ReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("quotes/xxxxx");
        
        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}