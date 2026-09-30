using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// A branch/location of a dealer business.
/// </summary>
public class DealerBranch : BaseEntity
{
    public Guid DealerId { get; set; }

    public Dealer? Dealer { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public bool IsActive { get; set; } = true;
}
