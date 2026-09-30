using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Aggregated search analytics data.
/// </summary>
public class SearchAnalytics : BaseEntity
{
    public string SearchQuery { get; set; } = string.Empty;
    public string? Filters { get; set; }
    public int ResultCount { get; set; }
    public Guid? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
