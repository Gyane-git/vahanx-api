using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Service type item within a service package.
/// </summary>
public class ServicePackageItem : BaseEntity
{
    public Guid ServicePackageId { get; set; }

    public ServicePackage? ServicePackage { get; set; }

    public Guid AutoServiceTypeId { get; set; }

    public AutoServiceType? AutoServiceType { get; set; }

    public int? Quantity { get; set; }

    public string? Notes { get; set; }
}
