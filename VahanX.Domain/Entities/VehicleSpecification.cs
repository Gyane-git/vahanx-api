using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle specification definition (Engine Capacity, Power, Torque, etc.)
/// </summary>
public class VehicleSpecification : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Unit { get; set; } = string.Empty;

    public string DataType { get; set; } = "string";

    public string? SpecGroup { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<Variant> Variants { get; set; } = [];
}
