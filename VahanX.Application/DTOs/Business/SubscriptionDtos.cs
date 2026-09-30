using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Business;

/// <summary>
/// Feature response DTO.
/// </summary>
public class FeatureResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public FeatureType FeatureType { get; set; }
    public string? Unit { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Create feature request.
/// </summary>
public class CreateFeatureRequest
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public FeatureType FeatureType { get; set; } = FeatureType.Boolean;
    public string? Unit { get; set; }
}

/// <summary>
/// Update feature request.
/// </summary>
public class UpdateFeatureRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public FeatureType? FeatureType { get; set; }
    public string? Unit { get; set; }
    public bool? IsActive { get; set; }
}

/// <summary>
/// Subscription plan response DTO.
/// </summary>
public class SubscriptionPlanResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SubscriptionTargetType TargetType { get; set; }
    public BillingCycle BillingCycle { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int TrialDays { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublic { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Create subscription plan request.
/// </summary>
public class CreateSubscriptionPlanRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SubscriptionTargetType TargetType { get; set; } = SubscriptionTargetType.User;
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "NPR";
    public int TrialDays { get; set; }
    public bool IsPublic { get; set; } = true;
}

/// <summary>
/// Update subscription plan request.
/// </summary>
public class UpdateSubscriptionPlanRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? TrialDays { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPublic { get; set; }
}

/// <summary>
/// Subscription response DTO.
/// </summary>
public class SubscriptionResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? SellerId { get; set; }
    public Guid? DealerId { get; set; }
    public Guid SubscriptionPlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public SubscriptionStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool AutoRenew { get; set; }
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }
}

/// <summary>
/// Create subscription request.
/// </summary>
public class CreateSubscriptionRequest
{
    public Guid SubscriptionPlanId { get; set; }
}

/// <summary>
/// Change plan request.
/// </summary>
public class ChangePlanRequest
{
    public Guid NewPlanId { get; set; }
    public string? Reason { get; set; }
}

/// <summary>
/// Subscription usage response DTO.
/// </summary>
public class SubscriptionUsageResponse
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public Guid FeatureId { get; set; }
    public string FeatureCode { get; set; } = string.Empty;
    public string FeatureName { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public int UsedValue { get; set; }
    public int LimitValue { get; set; }
}
