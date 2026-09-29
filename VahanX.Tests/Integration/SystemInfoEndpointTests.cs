using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using VahanX.Application.Common;
using VahanX.Application.DTOs;

namespace VahanX.Tests.Integration;

/// <summary>
/// Integration tests for the system info endpoint.
/// </summary>
public class SystemInfoEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SystemInfoEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ApplicationName"] = "VahanX API",
                    ["ApiVersion"] = "v1",
                    ["Environment"] = "Testing"
                });
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetSystemInfo_ShouldReturnSystemInfo()
    {
        var response = await _client.GetAsync("/api/v1/system/info");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<SystemInfoDto>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
        Assert.NotNull(content.Data);
        Assert.Equal("VahanX API", content.Data.ApplicationName);
        Assert.Equal("v1", content.Data.ApiVersion);
        Assert.NotEqual(default, content.Data.CurrentUtcTime);
    }

    [Fact]
    public async Task GetSystemInfo_ShouldReturnCorrectApiVersion()
    {
        var response = await _client.GetAsync("/api/v1/system/info");

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<SystemInfoDto>>();
        Assert.NotNull(content);
        Assert.Equal("v1", content.Data!.ApiVersion);
    }
}
