using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Business;

/// <summary>
/// Advertisement campaign response DTO.
/// </summary>
public class AdvertisementCampaignResponse
{
    public Guid Id { get; set; }
    public Guid AdvertiserUserId { get; set; }
    public Guid? SellerId { get; set; }
    public Guid? DealerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AdvertisementCampaignStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string BudgetType { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
}

/// <summary>
/// Create advertisement campaign request.
/// </summary>
public class CreateAdvertisementCampaignRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string BudgetType { get; set; } = "Total";
    public decimal BudgetAmount { get; set; }
    public decimal? DailyBudget { get; set; }
    public decimal? TotalBudget { get; set; }
    public string Currency { get; set; } = "NPR";
}

/// <summary>
/// Update advertisement campaign request.
/// </summary>
public class UpdateAdvertisementCampaignRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? BudgetAmount { get; set; }
}

/// <summary>
/// Advertisement response DTO.
/// </summary>
public class AdvertisementResponse
{
    public Guid Id { get; set; }
    public Guid CampaignId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Headline { get; set; }
    public string? Description { get; set; }
    public AdvertisementDestinationType DestinationType { get; set; }
    public string? DestinationUrl { get; set; }
    public Guid? VehicleListingId { get; set; }
    public string? MediaReference { get; set; }
    public AdvertisementStatus Status { get; set; }
}

/// <summary>
/// Create advertisement request.
/// </summary>
public class CreateAdvertisementRequest
{
    public Guid CampaignId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Headline { get; set; }
    public string? Description { get; set; }
    public AdvertisementDestinationType DestinationType { get; set; } = AdvertisementDestinationType.Listing;
    public string? DestinationUrl { get; set; }
    public Guid? VehicleListingId { get; set; }
    public string? MediaReference { get; set; }
}

/// <summary>
/// Update advertisement request.
/// </summary>
public class UpdateAdvertisementRequest
{
    public string? Name { get; set; }
    public string? Headline { get; set; }
    public string? Description { get; set; }
    public string? DestinationUrl { get; set; }
    public string? MediaReference { get; set; }
}
