using VahanX.Application.Common;

namespace VahanX.Tests.Unit;

/// <summary>
/// Unit tests for the standard API response.
/// </summary>
public class ApiResponseTests
{
    [Fact]
    public void SuccessResponse_ShouldReturnSuccessResult()
    {
        var data = new { Name = "Test" };
        var response = ApiResponse<object>.SuccessResponse(data, "Success");

        Assert.True(response.Success);
        Assert.Equal("Success", response.Message);
        Assert.NotNull(response.Data);
        Assert.Empty(response.Errors);
        Assert.NotEmpty(response.TraceId);
    }

    [Fact]
    public void ErrorResponse_ShouldReturnErrorResult()
    {
        var response = ApiResponse<object>.ErrorResponse("Error occurred");

        Assert.False(response.Success);
        Assert.Equal("Error occurred", response.Message);
        Assert.Null(response.Data);
        Assert.Empty(response.Errors);
        Assert.NotEmpty(response.TraceId);
    }

    [Fact]
    public void ErrorResponse_WithErrors_ShouldReturnErrors()
    {
        var errors = new List<ApiError>
        {
            new() { Code = "VALIDATION_ERROR", Field = "Name", Message = "Name is required" }
        };

        var response = ApiResponse<object>.ErrorResponse("Validation failed", errors);

        Assert.False(response.Success);
        Assert.Single(response.Errors);
        Assert.Equal("VALIDATION_ERROR", response.Errors[0].Code);
        Assert.Equal("Name", response.Errors[0].Field);
    }

    [Fact]
    public void ErrorResponse_WithSingleError_ShouldReturnSingleError()
    {
        var response = ApiResponse<object>.ErrorResponse(
            "Validation failed", "VALIDATION_ERROR", "Email", "Email is invalid");

        Assert.False(response.Success);
        Assert.Single(response.Errors);
        Assert.Equal("VALIDATION_ERROR", response.Errors[0].Code);
        Assert.Equal("Email", response.Errors[0].Field);
        Assert.Equal("Email is invalid", response.Errors[0].Message);
    }
}
