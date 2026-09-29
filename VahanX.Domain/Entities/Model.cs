using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle model belonging to a Brand.
/// </summary>
public class Model : BaseEntity
{
    public Guid BrandId { get; set; }

    public Brand? Brand { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<Generation> Generations { get; set; } = [];
}
