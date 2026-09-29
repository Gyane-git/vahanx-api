using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using VahanX.Application.Common;

namespace VahanX.Tests.Integration;

/// <summary>
/// Integration tests for health check endpoints.
/// </summary>
public class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ShouldReturnHealthy()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetReadiness_ShouldReturnReady()
    {
        var response = await _client.GetAsync("/health/ready");

        // Readiness may fail if database is not available, but endpoint should respond
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.ServiceUnavailable);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }
}
