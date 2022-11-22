namespace Knab.CryptoQuote.Domain;

public readonly record struct Quote(CurrencyCode CurrencyCode, decimal Price);