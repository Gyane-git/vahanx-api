using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle category master data belonging to a VehicleType.
/// </summary>
public class VehicleCategory : BaseEntity
{
    public Guid VehicleTypeId { get; set; }

    public VehicleType? VehicleType { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }
}
