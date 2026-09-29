namespace VahanX.Application.DTOs.VehicleCore;

/// <summary>
/// Variant response DTO.
/// </summary>
public class VariantDto
{
    public Guid Id { get; set; }
    public Guid GenerationId { get; set; }
    public string GenerationName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ModelYearFrom { get; set; }
    public int? ModelYearTo { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Create variant request.
/// </summary>
public class CreateVariantRequest
{
    public Guid GenerationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ModelYearFrom { get; set; }
    public int? ModelYearTo { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Update variant request.
/// </summary>
public class UpdateVariantRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ModelYearFrom { get; set; }
    public int? ModelYearTo { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}
