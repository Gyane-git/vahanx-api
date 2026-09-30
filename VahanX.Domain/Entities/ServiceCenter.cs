using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Service center business/company.
/// A service center can have multiple branches.
/// </summary>
public class ServiceCenter : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid? OwnerUserId { get; set; }

    public Guid? SellerId { get; set; }

    public Seller? Seller { get; set; }

    public Guid? DealerId { get; set; }

    public Dealer? Dealer { get; set; }

    public string? BusinessRegistrationNumber { get; set; }

    public string ContactPhone { get; set; } = string.Empty;

    public string? ContactEmail { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? LogoMediaReference { get; set; }

    public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Unverified;

    public ServiceCenterStatus Status { get; set; } = ServiceCenterStatus.PendingApproval;

    public ICollection<ServiceCenterBranch> Branches { get; set; } = new List<ServiceCenterBranch>();
}
