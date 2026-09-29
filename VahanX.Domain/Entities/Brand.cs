using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle brand master data (Toyota, Honda, Yamaha, etc.)
/// </summary>
public class Brand : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid? LogoMediaId { get; set; }

    public string? CountryOfOrigin { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<Model> Models { get; set; } = [];
}
