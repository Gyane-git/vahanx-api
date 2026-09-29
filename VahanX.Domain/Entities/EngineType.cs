using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Engine type master data (ICE, Electric Motor, Hybrid, etc.)
/// </summary>
public class EngineType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = [];
}
