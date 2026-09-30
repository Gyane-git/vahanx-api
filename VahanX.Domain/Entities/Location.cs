using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Geographic location for marketplace listings and dealer branches.
/// Supports nearby search and map display.
/// </summary>
public class Location : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Province { get; set; }

    public string? District { get; set; }

    public string? Municipality { get; set; }

    public string? Ward { get; set; }

    public string? StreetAddress { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}
