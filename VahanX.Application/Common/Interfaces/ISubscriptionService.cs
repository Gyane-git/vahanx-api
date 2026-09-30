using VahanX.Application.Common;
using VahanX.Application.DTOs.Business;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for subscription operations.
/// </summary>
public interface ISubscriptionService
{
    Task<PagedResult<SubscriptionPlanResponse>> GetPlansAsync(int page, int pageSize, bool? isActive = null, CancellationToken cancellationToken = default);
    Task<SubscriptionPlanResponse?> GetPlanByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SubscriptionPlanResponse> CreatePlanAsync(CreateSubscriptionPlanRequest request, CancellationToken cancellationToken = default);
    Task<SubscriptionPlanResponse> UpdatePlanAsync(Guid id, UpdateSubscriptionPlanRequest request, CancellationToken cancellationToken = default);
    Task<SubscriptionResponse?> GetMySubscriptionAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<SubscriptionResponse?> GetSubscriptionByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<SubscriptionResponse> CancelSubscriptionAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<SubscriptionResponse> RenewSubscriptionAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<SubscriptionResponse> ChangePlanAsync(Guid id, ChangePlanRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<SubscriptionUsageResponse>> GetSubscriptionUsageAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
}
