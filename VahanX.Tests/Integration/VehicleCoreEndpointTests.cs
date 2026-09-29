using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using VahanX.Application.Common;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Tests.Integration;

/// <summary>
/// Integration tests for Vehicle Core endpoints.
/// </summary>
public class VehicleCoreEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public VehicleCoreEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetVehicleTypes_ShouldReturnPagedResult()
    {
        var response = await _client.GetAsync("/api/v1/vehicle-types?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<VehicleTypeDto>>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
        Assert.NotNull(content.Data);
    }

    [Fact]
    public async Task GetBrands_ShouldReturnPagedResult()
    {
        var response = await _client.GetAsync("/api/v1/brands?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<BrandDto>>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
    }

    [Fact]
    public async Task GetModels_ShouldReturnPagedResult()
    {
        var response = await _client.GetAsync("/api/v1/models?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<ModelDto>>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
    }

    [Fact]
    public async Task GetVehicles_ShouldReturnPagedResult()
    {
        var response = await _client.GetAsync("/api/v1/vehicles?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<VehicleDto>>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
    }

    [Fact]
    public async Task GetBodyTypes_ShouldReturnPagedResult()
    {
        var response = await _client.GetAsync("/api/v1/master-data/body-types?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<BodyTypeDto>>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
    }

    [Fact]
    public async Task GetFuelTypes_ShouldReturnPagedResult()
    {
        var response = await _client.GetAsync("/api/v1/master-data/fuel-types?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<FuelTypeDto>>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
    }

    [Fact]
    public async Task GetVehicleTypes_WithSearch_ShouldReturnFilteredResults()
    {
        var response = await _client.GetAsync("/api/v1/vehicle-types?search=car&page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<VehicleTypeDto>>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
    }
}
