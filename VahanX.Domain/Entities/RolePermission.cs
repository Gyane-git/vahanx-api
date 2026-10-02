using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Join entity between Role and Permission.
/// </summary>
public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}
