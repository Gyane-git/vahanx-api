using VahanX.Domain.Exceptions;

namespace VahanX.Tests.Unit;

/// <summary>
/// Unit tests for domain exceptions.
/// </summary>
public class DomainExceptionTests
{
    [Fact]
    public void NotFoundException_ShouldHaveCorrectErrorCode()
    {
        var ex = new NotFoundException("Vehicle", "123");

        Assert.Equal("NOT_FOUND", ex.ErrorCode);
        Assert.Contains("Vehicle", ex.Message);
        Assert.Contains("123", ex.Message);
    }

    [Fact]
    public void ValidationException_ShouldHaveCorrectErrorCode()
    {
        var errors = new Dictionary<string, string[]>
        {
            { "Name", ["Name is required"] }
        };

        var ex = new ValidationException("Validation failed", errors);

        Assert.Equal("VALIDATION_ERROR", ex.ErrorCode);
        Assert.Single(ex.Errors);
        Assert.Contains("Name", ex.Errors.Keys);
    }

    [Fact]
    public void ConflictException_ShouldHaveCorrectErrorCode()
    {
        var ex = new ConflictException("Duplicate entry");

        Assert.Equal("CONFLICT", ex.ErrorCode);
        Assert.Equal("Duplicate entry", ex.Message);
    }

    [Fact]
    public void UnauthorizedException_ShouldHaveCorrectErrorCode()
    {
        var ex = new UnauthorizedException("Unauthorized");

        Assert.Equal("UNAUTHORIZED", ex.ErrorCode);
    }

    [Fact]
    public void ForbiddenException_ShouldHaveCorrectErrorCode()
    {
        var ex = new ForbiddenException("Forbidden");

        Assert.Equal("FORBIDDEN", ex.ErrorCode);
    }
}
