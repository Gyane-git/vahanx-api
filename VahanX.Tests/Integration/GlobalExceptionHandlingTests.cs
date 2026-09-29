using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using VahanX.Application.Common;

namespace VahanX.Tests.Integration;

/// <summary>
/// Integration tests for global exception handling.
/// </summary>
public class GlobalExceptionHandlingTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GlobalExceptionHandlingTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task InvalidRoute_ShouldReturn404()
    {
        var response = await _client.GetAsync("/api/v1/nonexistent");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Response_ShouldHaveCorrectContentType()
    {
        var response = await _client.GetAsync("/api/v1/system/info");

        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }
}
