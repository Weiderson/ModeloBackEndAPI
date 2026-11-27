using DesafioBackendAPI.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Xunit;

namespace DesafioBackendAPI.Tests;

public class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task TryHandleAsync_ReturnsProblemDetailsWithoutExceptionMessage()
    {
        using var services = new ServiceCollection()
            .AddLogging()
            .AddProblemDetails()
            .BuildServiceProvider();
        var context = new DefaultHttpContext
        {
            RequestServices = services
        };
        context.Request.Path = "/api/private";
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        var handler = new GlobalExceptionHandler(
            NullLogger<GlobalExceptionHandler>.Instance,
            services.GetRequiredService<IProblemDetailsService>());

        var handled = await handler.TryHandleAsync(
            context,
            new InvalidOperationException("sensitive database details"),
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);

        responseBody.Position = 0;
        using var document = await JsonDocument.ParseAsync(responseBody);
        Assert.Equal(StatusCodes.Status500InternalServerError, document.RootElement.GetProperty("status").GetInt32());
        Assert.DoesNotContain("sensitive database details", document.RootElement.ToString());
    }

    [Fact]
    public async Task TryHandleAsync_MapsMissingResourcesToNotFound()
    {
        using var services = new ServiceCollection()
            .AddLogging()
            .AddProblemDetails()
            .BuildServiceProvider();
        var context = new DefaultHttpContext
        {
            RequestServices = services
        };
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        var handler = new GlobalExceptionHandler(
            NullLogger<GlobalExceptionHandler>.Instance,
            services.GetRequiredService<IProblemDetailsService>());

        await handler.TryHandleAsync(
            context,
            new KeyNotFoundException("internal resource identifier"),
            CancellationToken.None);

        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        responseBody.Position = 0;
        using var document = await JsonDocument.ParseAsync(responseBody);
        Assert.DoesNotContain("internal resource identifier", document.RootElement.ToString());
    }
}
