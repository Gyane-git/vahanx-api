using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Business;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of subscription service.
/// </summary>
public class SubscriptionService : ISubscriptionService
{
    private readonly IRepository<SubscriptionPlan> _planRepository;
    private readonly IRepository<Subscription> _subscriptionRepository;
    private readonly IRepository<SubscriptionUsage> _usageRepository;
    private readonly IRepository<SubscriptionChangeHistory> _historyRepository;

    public SubscriptionService(
        IRepository<SubscriptionPlan> planRepository,
        IRepository<Subscription> subscriptionRepository,
        IRepository<SubscriptionUsage> usageRepository,
        IRepository<SubscriptionChangeHistory> historyRepository)
    {
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
        _usageRepository = usageRepository;
        _historyRepository = historyRepository;
    }

    public async Task<PagedResult<SubscriptionPlanResponse>> GetPlansAsync(int page, int pageSize, bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var query = await _planRepository.QueryAsync(true, cancellationToken);

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(p => p.DisplayOrder)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new SubscriptionPlanResponse
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
                TargetType = p.TargetType,
                BillingCycle = p.BillingCycle,
                Price = p.Price,
                Currency = p.Currency,
                TrialDays = p.TrialDays,
                IsActive = p.IsActive,
                IsPublic = p.IsPublic,
                DisplayOrder = p.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        return PagedResult<SubscriptionPlanResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<SubscriptionPlanResponse?> GetPlanByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var plan = await _planRepository.GetByIdAsync(id, cancellationToken);
        if (plan is null) return null;

        return new SubscriptionPlanResponse
        {
            Id = plan.Id,
            Name = plan.Name,
            Code = plan.Code,
            Description = plan.Description,
            TargetType = plan.TargetType,
            BillingCycle = plan.BillingCycle,
            Price = plan.Price,
            Currency = plan.Currency,
            TrialDays = plan.TrialDays,
            IsActive = plan.IsActive,
            IsPublic = plan.IsPublic,
            DisplayOrder = plan.DisplayOrder
        };
    }

    public async Task<SubscriptionPlanResponse> CreatePlanAsync(CreateSubscriptionPlanRequest request, CancellationToken cancellationToken = default)
    {
        var plan = new SubscriptionPlan
        {
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            TargetType = request.TargetType,
            BillingCycle = request.BillingCycle,
            Price = request.Price,
            Currency = request.Currency,
            TrialDays = request.TrialDays,
            IsPublic = request.IsPublic
        };

        await _planRepository.AddAsync(plan, cancellationToken);

        return new SubscriptionPlanResponse
        {
            Id = plan.Id,
            Name = plan.Name,
            Code = plan.Code,
            Description = plan.Description,
            TargetType = plan.TargetType,
            BillingCycle = plan.BillingCycle,
            Price = plan.Price,
            Currency = plan.Currency,
            TrialDays = plan.TrialDays,
            IsActive = plan.IsActive,
            IsPublic = plan.IsPublic,
            DisplayOrder = plan.DisplayOrder
        };
    }

    public async Task<SubscriptionPlanResponse> UpdatePlanAsync(Guid id, UpdateSubscriptionPlanRequest request, CancellationToken cancellationToken = default)
    {
        var plan = await _planRepository.GetByIdAsync(id, cancellationToken);
        if (plan is null) throw new NotFoundException("SubscriptionPlan", id);

        if (request.Name != null) plan.Name = request.Name;
        if (request.Description != null) plan.Description = request.Description;
        if (request.Price.HasValue) plan.Price = request.Price.Value;
        if (request.TrialDays.HasValue) plan.TrialDays = request.TrialDays.Value;
        if (request.IsActive.HasValue) plan.IsActive = request.IsActive.Value;
        if (request.IsPublic.HasValue) plan.IsPublic = request.IsPublic.Value;

        await _planRepository.UpdateAsync(plan, cancellationToken);

        return new SubscriptionPlanResponse
        {
            Id = plan.Id,
            Name = plan.Name,
            Code = plan.Code,
            Description = plan.Description,
            TargetType = plan.TargetType,
            BillingCycle = plan.BillingCycle,
            Price = plan.Price,
            Currency = plan.Currency,
            TrialDays = plan.TrialDays,
            IsActive = plan.IsActive,
            IsPublic = plan.IsPublic,
            DisplayOrder = plan.DisplayOrder
        };
    }

    public async Task<SubscriptionResponse?> GetMySubscriptionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _subscriptionRepository.QueryAsync(true, cancellationToken);
        var subscription = await subscriptions
            .Where(s => s.UserId == userId && s.Status == SubscriptionStatus.Active)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (subscription is null) return null;

        return new SubscriptionResponse
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            SellerId = subscription.SellerId,
            DealerId = subscription.DealerId,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            PlanName = subscription.SubscriptionPlan != null ? subscription.SubscriptionPlan.Name : string.Empty,
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            AutoRenew = subscription.AutoRenew,
            CurrentPeriodStart = subscription.CurrentPeriodStart,
            CurrentPeriodEnd = subscription.CurrentPeriodEnd
        };
    }

    public async Task<SubscriptionResponse?> GetSubscriptionByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription is null) return null;

        if (subscription.UserId != userId)
            throw new ForbiddenException("You do not have access to this subscription.");

        return new SubscriptionResponse
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            SellerId = subscription.SellerId,
            DealerId = subscription.DealerId,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            PlanName = subscription.SubscriptionPlan != null ? subscription.SubscriptionPlan.Name : string.Empty,
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            AutoRenew = subscription.AutoRenew,
            CurrentPeriodStart = subscription.CurrentPeriodStart,
            CurrentPeriodEnd = subscription.CurrentPeriodEnd
        };
    }

    public async Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var plan = await _planRepository.GetByIdAsync(request.SubscriptionPlanId, cancellationToken);
        if (plan is null) throw new NotFoundException("SubscriptionPlan", request.SubscriptionPlanId);

        if (!plan.IsActive)
            throw new ConflictException("Subscription plan is not active.");

        var subscription = new Subscription
        {
            UserId = userId,
            SubscriptionPlanId = plan.Id,
            Status = plan.TrialDays > 0 ? SubscriptionStatus.Trialing : SubscriptionStatus.Active,
            StartDate = DateTime.UtcNow,
            TrialStartDate = plan.TrialDays > 0 ? DateTime.UtcNow : null,
            TrialEndDate = plan.TrialDays > 0 ? DateTime.UtcNow.AddDays(plan.TrialDays) : null,
            CurrentPeriodStart = DateTime.UtcNow,
            CurrentPeriodEnd = CalculatePeriodEnd(DateTime.UtcNow, plan.BillingCycle)
        };

        await _subscriptionRepository.AddAsync(subscription, cancellationToken);

        await AddChangeHistoryAsync(subscription.Id, null, SubscriptionStatus.Pending, SubscriptionStatus.Active, "Subscription created", userId, cancellationToken);

        return new SubscriptionResponse
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            SellerId = subscription.SellerId,
            DealerId = subscription.DealerId,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            PlanName = plan.Name,
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            AutoRenew = subscription.AutoRenew,
            CurrentPeriodStart = subscription.CurrentPeriodStart,
            CurrentPeriodEnd = subscription.CurrentPeriodEnd
        };
    }

    public async Task<SubscriptionResponse> CancelSubscriptionAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription is null) throw new NotFoundException("Subscription", id);

        if (subscription.UserId != userId)
            throw new ForbiddenException("You can only cancel your own subscriptions.");

        if (subscription.Status == SubscriptionStatus.Cancelled || subscription.Status == SubscriptionStatus.Expired)
            throw new ConflictException($"Cannot cancel a subscription with status {subscription.Status}.");

        var oldStatus = subscription.Status;
        subscription.Status = SubscriptionStatus.Cancelled;
        subscription.CancelledAt = DateTime.UtcNow;
        subscription.AutoRenew = false;
        await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);

        await AddChangeHistoryAsync(subscription.Id, subscription.SubscriptionPlanId, oldStatus, SubscriptionStatus.Cancelled, "User cancelled", userId, cancellationToken);

        return new SubscriptionResponse
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            SellerId = subscription.SellerId,
            DealerId = subscription.DealerId,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            PlanName = string.Empty,
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            AutoRenew = subscription.AutoRenew,
            CurrentPeriodStart = subscription.CurrentPeriodStart,
            CurrentPeriodEnd = subscription.CurrentPeriodEnd
        };
    }

    public async Task<SubscriptionResponse> RenewSubscriptionAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription is null) throw new NotFoundException("Subscription", id);

        if (subscription.UserId != userId)
            throw new ForbiddenException("You can only renew your own subscriptions.");

        if (subscription.Status != SubscriptionStatus.Active && subscription.Status != SubscriptionStatus.PastDue)
            throw new ConflictException($"Cannot renew a subscription with status {subscription.Status}.");

        var oldStatus = subscription.Status;
        subscription.Status = SubscriptionStatus.Active;
        subscription.CurrentPeriodStart = DateTime.UtcNow;
        subscription.CurrentPeriodEnd = CalculatePeriodEnd(DateTime.UtcNow, subscription.SubscriptionPlan?.BillingCycle ?? BillingCycle.Monthly);
        await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);

        await AddChangeHistoryAsync(subscription.Id, subscription.SubscriptionPlanId, oldStatus, SubscriptionStatus.Active, "Subscription renewed", userId, cancellationToken);

        return new SubscriptionResponse
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            SellerId = subscription.SellerId,
            DealerId = subscription.DealerId,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            PlanName = string.Empty,
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            AutoRenew = subscription.AutoRenew,
            CurrentPeriodStart = subscription.CurrentPeriodStart,
            CurrentPeriodEnd = subscription.CurrentPeriodEnd
        };
    }

    public async Task<SubscriptionResponse> ChangePlanAsync(Guid id, ChangePlanRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription is null) throw new NotFoundException("Subscription", id);

        if (subscription.UserId != userId)
            throw new ForbiddenException("You can only change your own subscriptions.");

        var newPlan = await _planRepository.GetByIdAsync(request.NewPlanId, cancellationToken);
        if (newPlan is null) throw new NotFoundException("SubscriptionPlan", request.NewPlanId);

        var oldPlanId = subscription.SubscriptionPlanId;
        var oldStatus = subscription.Status;
        subscription.SubscriptionPlanId = newPlan.Id;
        await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);

        await AddChangeHistoryAsync(subscription.Id, oldPlanId, oldStatus, subscription.Status, "Plan changed", userId, cancellationToken);

        return new SubscriptionResponse
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            SellerId = subscription.SellerId,
            DealerId = subscription.DealerId,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            PlanName = newPlan.Name,
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            AutoRenew = subscription.AutoRenew,
            CurrentPeriodStart = subscription.CurrentPeriodStart,
            CurrentPeriodEnd = subscription.CurrentPeriodEnd
        };
    }

    public async Task<PagedResult<SubscriptionUsageResponse>> GetSubscriptionUsageAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription is null) throw new NotFoundException("Subscription", id);

        if (subscription.UserId != userId)
            throw new ForbiddenException("You do not have access to this subscription.");

        var usage = await _usageRepository.QueryAsync(true, cancellationToken);
        var subscriptionUsage = await usage
            .Where(u => u.SubscriptionId == id)
            .ToListAsync(cancellationToken);

        var items = subscriptionUsage.Select(u => new SubscriptionUsageResponse
        {
            Id = u.Id,
            SubscriptionId = u.SubscriptionId,
            FeatureId = u.FeatureId,
            FeatureCode = u.Feature != null ? u.Feature.Code : string.Empty,
            FeatureName = u.Feature != null ? u.Feature.Name : string.Empty,
            PeriodStart = u.PeriodStart,
            PeriodEnd = u.PeriodEnd,
            UsedValue = u.UsedValue,
            LimitValue = u.LimitValue
        }).ToList();

        return PagedResult<SubscriptionUsageResponse>.Create(items, items.Count, 1, items.Count);
    }

    private async Task AddChangeHistoryAsync(Guid subscriptionId, Guid? oldPlanId, SubscriptionStatus? oldStatus, SubscriptionStatus newStatus, string changeType, Guid? changedByUserId, CancellationToken cancellationToken)
    {
        var history = new SubscriptionChangeHistory
        {
            SubscriptionId = subscriptionId,
            OldPlanId = oldPlanId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            ChangeType = changeType,
            ChangedAt = DateTime.UtcNow,
            ChangedByUserId = changedByUserId
        };

        await _historyRepository.AddAsync(history, cancellationToken);
    }

    private static DateTime CalculatePeriodEnd(DateTime start, BillingCycle cycle)
    {
        return cycle switch
        {
            BillingCycle.Monthly => start.AddMonths(1),
            BillingCycle.Quarterly => start.AddMonths(3),
            BillingCycle.Yearly => start.AddYears(1),
            _ => start.AddMonths(1)
        };
    }
}
