using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Database-driven feature for subscription entitlements.
/// </summary>
public class Feature : BaseEntity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public FeatureType FeatureType { get; set; } = FeatureType.Boolean;

    public string? Unit { get; set; }

    public bool IsActive { get; set; } = true;
}
