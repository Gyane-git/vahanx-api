using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Advertisement campaign.
/// </summary>
public class AdvertisementCampaign : BaseEntity
{
    public Guid AdvertiserUserId { get; set; }

    public Guid? SellerId { get; set; }

    public Seller? Seller { get; set; }

    public Guid? DealerId { get; set; }

    public Dealer? Dealer { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public AdvertisementCampaignStatus Status { get; set; } = AdvertisementCampaignStatus.Draft;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string BudgetType { get; set; } = "Total";

    public decimal BudgetAmount { get; set; }

    public decimal? DailyBudget { get; set; }

    public decimal? TotalBudget { get; set; }

    public string Currency { get; set; } = "NPR";

    public ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();

    public ICollection<AdvertisementBudget> Budgets { get; set; } = new List<AdvertisementBudget>();
}
