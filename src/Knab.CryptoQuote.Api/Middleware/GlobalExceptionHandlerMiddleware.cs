using System.Net;
using System.Text.Json;
using Knab.CryptoQuote.Domain.Exceptions;

namespace Knab.CryptoQuote.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch
            {
                AppException _ => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException _ => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(new
            {
                error.Message
            });

            await response.WriteAsync(result);
        }
    }
}