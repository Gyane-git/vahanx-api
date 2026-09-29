using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle model generation (10th Gen, 11th Gen, etc.)
/// </summary>
public class Generation : BaseEntity
{
    public Guid ModelId { get; set; }

    public Model? Model { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public int StartYear { get; set; }

    public int? EndYear { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<Variant> Variants { get; set; } = [];
}
