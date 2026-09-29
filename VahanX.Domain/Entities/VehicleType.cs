using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle type master data (Car, Bike, Scooter, SUV, etc.)
/// Database-driven to allow admin management.
/// </summary>
public class VehicleType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<VehicleCategory> Categories { get; set; } = [];
}
