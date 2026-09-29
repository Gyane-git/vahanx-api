using VahanX.Domain.Common;
using VahanX.Domain.Entities;

namespace VahanX.Tests.Unit;

/// <summary>
/// Unit tests for the BaseEntity and related domain entities.
/// </summary>
public class BaseEntityTests
{
    [Fact]
    public void BaseEntity_ShouldHaveDefaultValues()
    {
        var entity = new AuditLog();

        Assert.Equal(Guid.Empty, entity.Id);
        Assert.Equal(default(DateTime), entity.CreatedAt);
        Assert.Null(entity.UpdatedAt);
        Assert.Null(entity.CreatedBy);
        Assert.Null(entity.UpdatedBy);
        Assert.False(entity.IsDeleted);
        Assert.Null(entity.DeletedAt);
        Assert.Null(entity.DeletedBy);
    }

    [Fact]
    public void BaseEntity_ShouldSupportAuditFields()
    {
        var entity = new AuditLog
        {
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "user-123",
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = "user-456"
        };

        Assert.True(entity.CreatedAt != default);
        Assert.Equal("user-123", entity.CreatedBy);
        Assert.True(entity.UpdatedAt != null);
        Assert.Equal("user-456", entity.UpdatedBy);
    }

    [Fact]
    public void BaseEntity_ShouldSupportSoftDelete()
    {
        var entity = new AuditLog
        {
            IsDeleted = true,
            DeletedAt = DateTime.UtcNow,
            DeletedBy = "user-123"
        };

        Assert.True(entity.IsDeleted);
        Assert.NotNull(entity.DeletedAt);
        Assert.Equal("user-123", entity.DeletedBy);
    }

    [Fact]
    public void AuditLog_ShouldHaveRequiredFields()
    {
        var auditLog = new AuditLog
        {
            UserId = "user-123",
            Action = Domain.Enums.AuditAction.Create,
            EntityName = "Vehicle",
            EntityId = "vehicle-123",
            Timestamp = DateTime.UtcNow
        };

        Assert.Equal("user-123", auditLog.UserId);
        Assert.Equal(Domain.Enums.AuditAction.Create, auditLog.Action);
        Assert.Equal("Vehicle", auditLog.EntityName);
        Assert.Equal("vehicle-123", auditLog.EntityId);
    }
}
