using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle variant belonging to a Generation.
/// </summary>
public class Variant : BaseEntity
{
    public Guid GenerationId { get; set; }

    public Generation? Generation { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int ModelYearFrom { get; set; }

    public int? ModelYearTo { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = [];

    public ICollection<VehicleFeature> Features { get; set; } = [];

    public ICollection<VehicleSpecification> Specifications { get; set; } = [];
}
