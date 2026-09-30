using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Predefined service package offered by a branch.
/// </summary>
public class ServicePackage : BaseEntity
{
    public Guid ServiceCenterBranchId { get; set; }

    public ServiceCenterBranch? ServiceCenterBranch { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = "NPR";

    public int? EstimatedDurationMinutes { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<ServicePackageItem> Items { get; set; } = new List<ServicePackageItem>();
}
