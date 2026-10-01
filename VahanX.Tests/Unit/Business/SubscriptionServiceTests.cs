
using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.Business;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Business;

/// <summary>
/// Unit tests for SubscriptionService.
/// </summary>
public class SubscriptionServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly SubscriptionService _service;

    public SubscriptionServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var planRepository = new Repository<SubscriptionPlan>(_context);
        var subscriptionRepository = new Repository<Subscription>(_context);
        var usageRepository = new Repository<SubscriptionUsage>(_context);
        var historyRepository = new Repository<SubscriptionChangeHistory>(_context);

        _service = new SubscriptionService(planRepository, subscriptionRepository, usageRepository, historyRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreatePlanAsync_WithValidRequest_CreatesPlan()
    {
        // Arrange
        var request = new CreateSubscriptionPlanRequest
        {
            Name = "Basic Seller",
            Code = "BASIC-SELLER",
            Price = 999,
            BillingCycle = BillingCycle.Monthly,
            TargetType = SubscriptionTargetType.Seller
        };

        // Act
        var result = await _service.CreatePlanAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Basic Seller", result.Name);
        Assert.Equal(999m, result.Price);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_WithValidPlan_CreatesSubscription()
    {
        // Arrange
        var plan = new SubscriptionPlan
        {
            Id = Guid.NewGuid(),
            Name = "Basic Seller",
            Code = "BASIC-SELLER",
            Price = 999,
            BillingCycle = BillingCycle.Monthly,
            TargetType = SubscriptionTargetType.Seller,
            IsActive = true
        };
        await _context.SubscriptionPlans.AddAsync(plan);
        await _context.SaveChangesAsync();

        var request = new CreateSubscriptionRequest
        {
            SubscriptionPlanId = plan.Id
        };

        // Act
        var result = await _service.CreateSubscriptionAsync(request, Guid.NewGuid());

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(plan.Id, result.SubscriptionPlanId);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_WithInactivePlan_ThrowsConflictException()
    {
        // Arrange
        var plan = new SubscriptionPlan
        {
            Id = Guid.NewGuid(),
            Name = "Basic Seller",
            Code = "BASIC-SELLER",
            Price = 999,
            BillingCycle = BillingCycle.Monthly,
            TargetType = SubscriptionTargetType.Seller,
            IsActive = false
        };
        await _context.SubscriptionPlans.AddAsync(plan);
        await _context.SaveChangesAsync();

        var request = new CreateSubscriptionRequest
        {
            SubscriptionPlanId = plan.Id
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateSubscriptionAsync(request, Guid.NewGuid()));
    }

    [Fact]
    public async Task CancelSubscriptionAsync_WithActiveSubscription_ChangesToCancelled()
    {
        // Arrange
        var plan = new SubscriptionPlan
        {
            Id = Guid.NewGuid(),
            Name = "Basic Seller",
            Code = "BASIC-SELLER",
            Price = 999,
            BillingCycle = BillingCycle.Monthly,
            TargetType = SubscriptionTargetType.Seller,
            IsActive = true
        };
        await _context.SubscriptionPlans.AddAsync(plan);
        await _context.SaveChangesAsync();

        var userId = Guid.NewGuid();
        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            SubscriptionPlanId = plan.Id,
            Status = SubscriptionStatus.Active,
            StartDate = DateTime.UtcNow,
            CurrentPeriodStart = DateTime.UtcNow,
            CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1)
        };
        await _context.Subscriptions.AddAsync(subscription);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.CancelSubscriptionAsync(subscription.Id, userId);

        // Assert
        Assert.Equal(SubscriptionStatus.Cancelled, result.Status);
    }
}
