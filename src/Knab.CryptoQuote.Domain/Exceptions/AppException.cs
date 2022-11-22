﻿namespace Knab.CryptoQuote.Domain.Exceptions;

public class AppException : Exception
{
    public AppException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}