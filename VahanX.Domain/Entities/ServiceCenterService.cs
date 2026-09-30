using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Connects a service center branch to the services it provides.
/// </summary>
public class ServiceCenterService : BaseEntity
{
    public Guid ServiceCenterBranchId { get; set; }

    public ServiceCenterBranch? ServiceCenterBranch { get; set; }

    public Guid AutoServiceTypeId { get; set; }

    public AutoServiceType? AutoServiceType { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public int? EstimatedDurationMinutes { get; set; }

    public bool IsAvailable { get; set; } = true;

    public string? Description { get; set; }
}
