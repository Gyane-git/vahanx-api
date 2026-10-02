using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Role entity. System roles are protected from deletion/modification.
/// </summary>
public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; }
    public bool IsActive { get; set; } = true;
}
