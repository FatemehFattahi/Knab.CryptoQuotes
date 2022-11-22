using FluentAssertions;
using Knab.CryptoQuote.Domain;

namespace Knab.CryptoQuote.UnitTests;

public class CryptoCurrencyTests
{
    [Fact]
    public void InitializingWithValidCode_Should_Succeed()
    {
        // Arrange & Act
        var cryptoCurrency = new CryptoCurrency("BTC", new List<Quote>
        {
            new(CurrencyCode.USD, 10000)
        });

        // Assert
        cryptoCurrency.CryptoCurrencyCode.Should().Be("BTC");
        cryptoCurrency.Quotes.Should().HaveCount(1);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void InitializingWithEmptyCode_Should_Fail(string cryptoCode)
    {
        // Arrange & Act
        var action = () => new CryptoCurrency(cryptoCode, new List<Quote>());

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}