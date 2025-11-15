using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using RentCarServer.Application.Behaviors;
using System.Text.Encodings.Web;
using System.Text.Json;
using TS.Result;

namespace RentCarServer.WebAPI;

public sealed class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 500;

        var actualException = exception is AggregateException agg && agg.InnerException != null
            ? agg.InnerException
            : exception;

        // 🔸 FluentValidation hatası
        if (actualException is ValidationException validationException)
        {
            httpContext.Response.StatusCode = 422;

            // Her error için yalnızca ErrorMessage alıyoruz
            var validationMessages = validationException.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            var errorResult = Result<string>.Failure(422, validationMessages);

            var jsonOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            await httpContext.Response.WriteAsJsonAsync(errorResult, jsonOptions, cancellationToken);
            return true;
        }

        // 🔸 Authorization hatası
        if (actualException is AuthorizationException)
        {
            httpContext.Response.StatusCode = 403;

            var errorResult = Result<string>.Failure(403, "Bu işlem için yetkiniz yok.");

            await httpContext.Response.WriteAsJsonAsync(errorResult, cancellationToken);
            return true;
        }

        // 🔸 Diğer tüm hatalar
        var generalError = Result<string>.Failure(actualException.Message);
        await httpContext.Response.WriteAsJsonAsync(generalError, cancellationToken);

        return true;
    }
}