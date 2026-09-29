using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Body type master data (Sedan, Hatchback, SUV, etc.)
/// </summary>
public class BodyType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = [];
}
