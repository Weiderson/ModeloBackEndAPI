using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException && cancellationToken.IsCancellationRequested)
            return true;

        var statusCode = exception switch
        {
            ArgumentException or System.ComponentModel.DataAnnotations.ValidationException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(
                exception,
                "Unhandled exception while processing request {TraceId}.",
                httpContext.TraceIdentifier);
        }
        else
        {
            _logger.LogInformation(
                "Request {TraceId} failed with status code {StatusCode}.",
                httpContext.TraceIdentifier,
                statusCode);
        }

        httpContext.Response.StatusCode = statusCode;

        await _problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = statusCode switch
                {
                    StatusCodes.Status400BadRequest => "The request is invalid.",
                    StatusCodes.Status403Forbidden => "Access is forbidden.",
                    StatusCodes.Status404NotFound => "The requested resource was not found.",
                    StatusCodes.Status409Conflict => "The request conflicts with the current resource state.",
                    _ => "An unexpected error occurred."
                },
                Detail = statusCode >= StatusCodes.Status500InternalServerError
                    ? "The request could not be completed."
                    : null,
                Instance = httpContext.Request.Path
            }
        });

        return true;
    }
}
