namespace VahanX.Application.DTOs.VehicleCore;

/// <summary>
/// Brand response DTO.
/// </summary>
public class BrandDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? LogoMediaId { get; set; }
    public string? CountryOfOrigin { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Create brand request.
/// </summary>
public class CreateBrandRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? LogoMediaId { get; set; }
    public string? CountryOfOrigin { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Update brand request.
/// </summary>
public class UpdateBrandRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? LogoMediaId { get; set; }
    public string? CountryOfOrigin { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}
