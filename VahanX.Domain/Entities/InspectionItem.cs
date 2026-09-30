using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Individual inspection item within an inspection.
/// </summary>
public class InspectionItem : BaseEntity
{
    public Guid InspectionId { get; set; }

    public Inspection? Inspection { get; set; }

    public string Category { get; set; } = string.Empty;

    public string ItemName { get; set; } = string.Empty;

    public InspectionItemCondition Condition { get; set; } = InspectionItemCondition.NotInspected;

    public decimal? Score { get; set; }

    public string? Notes { get; set; }
}
