using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Reusable vehicle feature master data (ABS, Airbags, Sunroof, etc.)
/// </summary>
public class VehicleFeature : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string FeatureGroup { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<Variant> Variants { get; set; } = [];
}
