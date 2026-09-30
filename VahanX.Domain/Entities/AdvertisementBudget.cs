using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Budget tracking for an advertisement campaign.
/// </summary>
public class AdvertisementBudget : BaseEntity
{
    public Guid CampaignId { get; set; }

    public AdvertisementCampaign? Campaign { get; set; }

    public string BudgetType { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public decimal SpentAmount { get; set; }

    public decimal RemainingAmount { get; set; }

    public string Currency { get; set; } = "NPR";

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }
}
