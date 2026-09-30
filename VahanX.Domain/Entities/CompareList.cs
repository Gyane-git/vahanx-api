using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// User's compare list for comparing vehicle listings.
/// </summary>
public class CompareList : BaseEntity
{
    public Guid UserId { get; set; }

    public ICollection<CompareItem> Items { get; set; } = new List<CompareItem>();
}
