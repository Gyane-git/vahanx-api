using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Moderation;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Moderation;

/// <summary>
/// Unit tests for ModerationService.
/// </summary>
public class ModerationServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly ModerationService _service;

    public ModerationServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var reports = new Repository<Report>(_context);
        var reportReasons = new Repository<ReportReason>(_context);
        var cases = new Repository<ModerationCase>(_context);
        var actions = new Repository<ModerationAction>(_context);
        var history = new Repository<ModerationHistory>(_context);
        var restrictions = new Repository<UserRestriction>(_context);
        var listings = new Repository<VehicleListing>(_context);
        var sellers = new Repository<Seller>(_context);
        var dealers = new Repository<Dealer>(_context);

        _service = new ModerationService(reports, reportReasons, cases, actions, history, restrictions, listings, sellers, dealers);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateReportAsync_WithValidRequest_CreatesReportInPendingStatus()
    {
        var reason = new ReportReason
        {
            Id = Guid.NewGuid(),
            Code = "SPAM",
            Name = "Spam",
            TargetType = TargetType.Listing,
            IsActive = true,
            DisplayOrder = 1
        };
        await _context.ReportReasons.AddAsync(reason);
        await _context.SaveChangesAsync();

        var request = new CreateReportRequest
        {
            TargetType = TargetType.Listing,
            TargetId = Guid.NewGuid(),
            ReportReasonId = reason.Id,
            Description = "Test report"
        };

        var result = await _service.CreateReportAsync(request, Guid.NewGuid());

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(ModerationReportStatus.Pending, result.Status);
        Assert.Equal("Test report", result.Description);
    }

    [Fact]
    public async Task CreateReportAsync_WithInvalidReason_ThrowsNotFoundException()
    {
        var request = new CreateReportRequest
        {
            TargetType = TargetType.Listing,
            TargetId = Guid.NewGuid(),
            ReportReasonId = Guid.NewGuid(),
            Description = "Test report"
        };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateReportAsync(request, Guid.NewGuid()));
    }

    [Fact]
    public async Task AssignReportAsync_WithPendingStatus_ChangesToUnderReview()
    {
        var report = new Report
        {
            Id = Guid.NewGuid(),
            ReporterUserId = Guid.NewGuid(),
            TargetType = TargetType.Listing,
            TargetId = Guid.NewGuid(),
            ReportReasonId = Guid.NewGuid(),
            Status = ModerationReportStatus.Pending
        };
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();

        var request = new AssignReportRequest { AssignedToUserId = Guid.NewGuid() };
        var result = await _service.AssignReportAsync(report.Id, request);

        Assert.Equal(ModerationReportStatus.UnderReview, result.Status);
        Assert.NotNull(result.AssignedToUserId);
    }

    [Fact]
    public async Task ResolveReportAsync_WithUnderReviewStatus_ChangesToResolved()
    {
        var report = new Report
        {
            Id = Guid.NewGuid(),
            ReporterUserId = Guid.NewGuid(),
            TargetType = TargetType.Listing,
            TargetId = Guid.NewGuid(),
            ReportReasonId = Guid.NewGuid(),
            Status = ModerationReportStatus.UnderReview
        };
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();

        var request = new ResolveReportRequest { ResolutionNote = "Resolved" };
        var result = await _service.ResolveReportAsync(report.Id, request);

        Assert.Equal(ModerationReportStatus.Resolved, result.Status);
        Assert.NotNull(result.ResolvedAt);
    }

    [Fact]
    public async Task CreateCaseAsync_WithValidRequest_CreatesCaseInOpenStatus()
    {
        var request = new CreateModerationCaseRequest
        {
            TargetType = TargetType.Listing,
            TargetId = Guid.NewGuid(),
            Priority = ReportPriority.Normal
        };

        var result = await _service.CreateCaseAsync(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(ModerationCaseStatus.Open, result.Status);
    }

    [Fact]
    public async Task CreateUserRestrictionAsync_WithValidRequest_CreatesActiveRestriction()
    {
        var request = new CreateUserRestrictionRequest
        {
            UserId = Guid.NewGuid(),
            RestrictionType = UserRestrictionType.Warning,
            Reason = "Test restriction"
        };

        var result = await _service.CreateUserRestrictionAsync(request, Guid.NewGuid());

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.True(result.IsActive);
        Assert.Equal(UserRestrictionType.Warning, result.RestrictionType);
    }

    [Fact]
    public async Task ApproveListingAsync_WithPendingReviewStatus_ChangesToPublished()
    {
        var listing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            Title = "Test Listing",
            Price = 1000000,
            Status = ListingStatus.PendingReview
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        var result = await _service.ApproveListingAsync(listing.Id, Guid.NewGuid());

        Assert.Equal(ListingStatus.Published, result.Status);
    }

    [Fact]
    public async Task ApproveListingAsync_WithInvalidStatus_ThrowsInvalidModerationTransitionException()
    {
        var listing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            Title = "Test Listing",
            Price = 1000000,
            Status = ListingStatus.Draft
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidModerationTransitionException>(() => _service.ApproveListingAsync(listing.Id, Guid.NewGuid()));
    }
}
