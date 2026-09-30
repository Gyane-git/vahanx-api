namespace VahanX.Application.DTOs.Marketplace;

/// <summary>
/// Location response DTO.
/// </summary>
public class LocationResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Municipality { get; set; }
    public string? Ward { get; set; }
    public string? StreetAddress { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

/// <summary>
/// Create location request.
/// </summary>
public class CreateLocationRequest
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

/// <summary>
/// Update location request.
/// </summary>
public class UpdateLocationRequest
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
