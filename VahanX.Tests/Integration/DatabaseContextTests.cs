using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VahanX.Infrastructure.Persistence;

namespace VahanX.Tests.Integration;

/// <summary>
/// Integration tests for database context registration and configuration.
/// </summary>
public class DatabaseContextTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DatabaseContextTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void DatabaseContext_ShouldBeRegistered()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<VahanXDbContext>();

        Assert.NotNull(context);
    }

    [Fact]
    public void DatabaseContext_ShouldHaveAuditLogDbSet()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<VahanXDbContext>();

        Assert.NotNull(context.AuditLogs);
    }

    [Fact]
    public void DatabaseContext_ShouldBeAbleToCreateInMemory()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new VahanXDbContext(options);
        Assert.NotNull(context);
    }
}
