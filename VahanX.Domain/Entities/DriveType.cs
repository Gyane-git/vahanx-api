using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Drive type master data (FWD, RWD, AWD, 4WD)
/// </summary>
public class DriveType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = [];
}
