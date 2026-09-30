using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// FAQ entity for public help content.
/// </summary>
public class FAQ : BaseEntity
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsPublished { get; set; }
}
