using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Permission entity representing an explicit capability in the system.
/// Permissions are system-defined and immutable through the management API.
/// </summary>
public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Module { get; set; } = string.Empty;
    public bool IsSystemDefined { get; set; } = true;
}
