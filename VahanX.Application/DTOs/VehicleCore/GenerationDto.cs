namespace VahanX.Application.DTOs.VehicleCore;

/// <summary>
/// Generation response DTO.
/// </summary>
public class GenerationDto
{
    public Guid Id { get; set; }
    public Guid ModelId { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public int? EndYear { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Create generation request.
/// </summary>
public class CreateGenerationRequest
{
    public Guid ModelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public int? EndYear { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Update generation request.
/// </summary>
public class UpdateGenerationRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public int? EndYear { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}
