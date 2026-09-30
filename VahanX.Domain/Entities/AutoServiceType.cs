using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Database-driven auto service type master data.
/// </summary>
public class AutoServiceType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid? VehicleTypeId { get; set; }

    public VehicleType? VehicleType { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }
}
