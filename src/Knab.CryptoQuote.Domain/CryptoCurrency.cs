namespace Knab.CryptoQuote.Domain;

public sealed class CryptoCurrency
{
    public CryptoCurrency(string cryptoCurrencyCode, IEnumerable<Quote> quotes)
    {
        if (string.IsNullOrWhiteSpace(cryptoCurrencyCode))
        {
            throw new ArgumentNullException(nameof(cryptoCurrencyCode));
        }

        CryptoCurrencyCode = cryptoCurrencyCode;
        Quotes = quotes;
    }
    
    public string CryptoCurrencyCode { get; }
    
    public IEnumerable<Quote> Quotes { get; }
}