using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Audit;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Audit;

/// <summary>
/// Unit tests for AuditService.
/// </summary>
public class AuditServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly AuditService _service;

    public AuditServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);
        var auditLogs = new Repository<AuditLog>(_context);
        _service = new AuditService(auditLogs);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task AuditAsync_WithValidRequest_CreatesAuditLog()
    {
        var request = new CreateAuditLogRequest
        {
            ActorUserId = Guid.NewGuid(),
            Action = AuditAction.Create,
            EntityType = "Vehicle",
            EntityId = "vehicle-123",
            CorrelationId = "corr-123"
        };

        var result = await _service.AuditAsync(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(AuditAction.Create, result.Action);
        Assert.Equal("Vehicle", result.EntityType);
    }

    [Fact]
    public async Task GetAuditLogsAsync_WithNoFilters_ReturnsAllLogs()
    {
        var log1 = new AuditLog
        {
            Id = Guid.NewGuid(),
            Action = AuditAction.Create,
            EntityType = "Vehicle",
            EntityId = "vehicle-123",
            Timestamp = DateTime.UtcNow,
            CorrelationId = "corr-123"
        };
        var log2 = new AuditLog
        {
            Id = Guid.NewGuid(),
            Action = AuditAction.Update,
            EntityType = "User",
            EntityId = "user-123",
            Timestamp = DateTime.UtcNow,
            CorrelationId = "corr-456"
        };
        await _context.AuditLogs.AddRangeAsync(log1, log2);
        await _context.SaveChangesAsync();

        var result = await _service.GetAuditLogsAsync(null);

        Assert.Equal(2, result.TotalCount);
    }

    [Fact]
    public async Task GetAuditLogByIdAsync_WithExistingId_ReturnsLog()
    {
        var log = new AuditLog
        {
            Id = Guid.NewGuid(),
            Action = AuditAction.Create,
            EntityType = "Vehicle",
            EntityId = "vehicle-123",
            Timestamp = DateTime.UtcNow,
            CorrelationId = "corr-123"
        };
        await _context.AuditLogs.AddAsync(log);
        await _context.SaveChangesAsync();

        var result = await _service.GetAuditLogByIdAsync(log.Id);

        Assert.NotNull(result);
        Assert.Equal(log.Id, result.Id);
    }

    [Fact]
    public async Task GetAuditLogByIdAsync_WithNonExistingId_ReturnsNull()
    {
        var result = await _service.GetAuditLogByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }
}
