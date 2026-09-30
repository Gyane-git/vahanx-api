using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Media item for an inspection.
/// </summary>
public class InspectionMedia : BaseEntity
{
    public Guid InspectionId { get; set; }

    public Inspection? Inspection { get; set; }

    public Guid? InspectionItemId { get; set; }

    public InspectionItem? InspectionItem { get; set; }

    public string MediaReference { get; set; } = string.Empty;

    public MediaType MediaType { get; set; } = MediaType.Image;

    public string? Caption { get; set; }

    public int DisplayOrder { get; set; }
}
