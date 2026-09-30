using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Moderation;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of moderation service.
/// </summary>
public class ModerationService : IModerationService
{
    private readonly IRepository<Report> _reports;
    private readonly IRepository<ReportReason> _reportReasons;
    private readonly IRepository<ModerationCase> _cases;
    private readonly IRepository<ModerationAction> _actions;
    private readonly IRepository<ModerationHistory> _history;
    private readonly IRepository<UserRestriction> _restrictions;
    private readonly IRepository<VehicleListing> _listings;
    private readonly IRepository<Seller> _sellers;
    private readonly IRepository<Dealer> _dealers;

    public ModerationService(
        IRepository<Report> reports,
        IRepository<ReportReason> reportReasons,
        IRepository<ModerationCase> cases,
        IRepository<ModerationAction> actions,
        IRepository<ModerationHistory> history,
        IRepository<UserRestriction> restrictions,
        IRepository<VehicleListing> listings,
        IRepository<Seller> sellers,
        IRepository<Dealer> dealers)
    {
        _reports = reports;
        _reportReasons = reportReasons;
        _cases = cases;
        _actions = actions;
        _history = history;
        _restrictions = restrictions;
        _listings = listings;
        _sellers = sellers;
        _dealers = dealers;
    }

    public async Task<PagedResult<ReportResponse>> GetReportsAsync(ModerationReportStatus? status, ReportPriority? priority, Guid? assignedToUserId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _reports.QueryAsync(true, cancellationToken);

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);
        if (priority.HasValue)
            query = query.Where(r => r.Priority == priority.Value);
        if (assignedToUserId.HasValue)
            query = query.Where(r => r.AssignedToUserId == assignedToUserId.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ReportResponse
            {
                Id = r.Id,
                ReporterUserId = r.ReporterUserId,
                TargetType = r.TargetType,
                TargetId = r.TargetId,
                ReportReasonId = r.ReportReasonId,
                Description = r.Description,
                Status = r.Status,
                Priority = r.Priority,
                AssignedToUserId = r.AssignedToUserId,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                ResolvedAt = r.ResolvedAt,
                ResolutionNote = r.ResolutionNote
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ReportResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ReportResponse?> GetReportByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var report = await _reports.GetByIdAsync(id, cancellationToken);
        if (report is null) return null;

        return new ReportResponse
        {
            Id = report.Id,
            ReporterUserId = report.ReporterUserId,
            TargetType = report.TargetType,
            TargetId = report.TargetId,
            ReportReasonId = report.ReportReasonId,
            Description = report.Description,
            Status = report.Status,
            Priority = report.Priority,
            AssignedToUserId = report.AssignedToUserId,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt,
            ResolvedAt = report.ResolvedAt,
            ResolutionNote = report.ResolutionNote
        };
    }

    public async Task<ReportResponse> CreateReportAsync(CreateReportRequest request, Guid reporterUserId, CancellationToken cancellationToken = default)
    {
        var reasonExists = await _reportReasons.AnyAsync(rr => rr.Id == request.ReportReasonId && rr.IsActive, cancellationToken);
        if (!reasonExists) throw new NotFoundException("Report reason", request.ReportReasonId);

        var report = new Report
        {
            ReporterUserId = reporterUserId,
            TargetType = request.TargetType,
            TargetId = request.TargetId,
            ReportReasonId = request.ReportReasonId,
            Description = request.Description,
            Status = ModerationReportStatus.Pending,
            Priority = ReportPriority.Normal
        };

        await _reports.AddAsync(report, cancellationToken);

        return new ReportResponse
        {
            Id = report.Id,
            ReporterUserId = report.ReporterUserId,
            TargetType = report.TargetType,
            TargetId = report.TargetId,
            ReportReasonId = report.ReportReasonId,
            Description = report.Description,
            Status = report.Status,
            Priority = report.Priority,
            CreatedAt = report.CreatedAt
        };
    }

    public async Task<ReportResponse> AssignReportAsync(Guid id, AssignReportRequest request, CancellationToken cancellationToken = default)
    {
        var report = await _reports.GetByIdAsync(id, cancellationToken);
        if (report is null) throw new ReportNotFoundException(id);

        if (report.Status != ModerationReportStatus.Pending && report.Status != ModerationReportStatus.UnderReview)
            throw new InvalidModerationTransitionException($"Cannot assign a report with status {report.Status}.");

        report.AssignedToUserId = request.AssignedToUserId;
        report.Status = ModerationReportStatus.UnderReview;
        await _reports.UpdateAsync(report, cancellationToken);

        return new ReportResponse
        {
            Id = report.Id,
            ReporterUserId = report.ReporterUserId,
            TargetType = report.TargetType,
            TargetId = report.TargetId,
            ReportReasonId = report.ReportReasonId,
            Description = report.Description,
            Status = report.Status,
            Priority = report.Priority,
            AssignedToUserId = report.AssignedToUserId,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt
        };
    }

    public async Task<ReportResponse> ResolveReportAsync(Guid id, ResolveReportRequest request, CancellationToken cancellationToken = default)
    {
        var report = await _reports.GetByIdAsync(id, cancellationToken);
        if (report is null) throw new ReportNotFoundException(id);

        if (report.Status == ModerationReportStatus.Resolved || report.Status == ModerationReportStatus.Rejected || report.Status == ModerationReportStatus.Dismissed)
            throw new InvalidModerationTransitionException($"Cannot resolve a report with status {report.Status}.");

        report.Status = ModerationReportStatus.Resolved;
        report.ResolvedAt = DateTime.UtcNow;
        report.ResolutionNote = request.ResolutionNote;
        await _reports.UpdateAsync(report, cancellationToken);

        return new ReportResponse
        {
            Id = report.Id,
            ReporterUserId = report.ReporterUserId,
            TargetType = report.TargetType,
            TargetId = report.TargetId,
            ReportReasonId = report.ReportReasonId,
            Description = report.Description,
            Status = report.Status,
            Priority = report.Priority,
            AssignedToUserId = report.AssignedToUserId,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt,
            ResolvedAt = report.ResolvedAt,
            ResolutionNote = report.ResolutionNote
        };
    }

    public async Task<ReportResponse> RejectReportAsync(Guid id, string reason, CancellationToken cancellationToken = default)
    {
        var report = await _reports.GetByIdAsync(id, cancellationToken);
        if (report is null) throw new ReportNotFoundException(id);

        if (report.Status == ModerationReportStatus.Resolved || report.Status == ModerationReportStatus.Rejected || report.Status == ModerationReportStatus.Dismissed)
            throw new InvalidModerationTransitionException($"Cannot reject a report with status {report.Status}.");

        report.Status = ModerationReportStatus.Rejected;
        report.ResolvedAt = DateTime.UtcNow;
        report.ResolutionNote = reason;
        await _reports.UpdateAsync(report, cancellationToken);

        return new ReportResponse
        {
            Id = report.Id,
            ReporterUserId = report.ReporterUserId,
            TargetType = report.TargetType,
            TargetId = report.TargetId,
            ReportReasonId = report.ReportReasonId,
            Description = report.Description,
            Status = report.Status,
            Priority = report.Priority,
            AssignedToUserId = report.AssignedToUserId,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt,
            ResolvedAt = report.ResolvedAt,
            ResolutionNote = report.ResolutionNote
        };
    }

    public async Task<ReportResponse> EscalateReportAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var report = await _reports.GetByIdAsync(id, cancellationToken);
        if (report is null) throw new ReportNotFoundException(id);

        if (report.Status == ModerationReportStatus.Resolved || report.Status == ModerationReportStatus.Rejected || report.Status == ModerationReportStatus.Dismissed)
            throw new InvalidModerationTransitionException($"Cannot escalate a report with status {report.Status}.");

        report.Status = ModerationReportStatus.Escalated;
        report.Priority = ReportPriority.High;
        await _reports.UpdateAsync(report, cancellationToken);

        return new ReportResponse
        {
            Id = report.Id,
            ReporterUserId = report.ReporterUserId,
            TargetType = report.TargetType,
            TargetId = report.TargetId,
            ReportReasonId = report.ReportReasonId,
            Description = report.Description,
            Status = report.Status,
            Priority = report.Priority,
            AssignedToUserId = report.AssignedToUserId,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt
        };
    }

    public async Task<PagedResult<ModerationCaseResponse>> GetCasesAsync(ModerationCaseStatus? status, ReportPriority? priority, Guid? assignedToUserId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _cases.QueryAsync(true, cancellationToken);

        if (status.HasValue)
            query = query.Where(mc => mc.Status == status.Value);
        if (priority.HasValue)
            query = query.Where(mc => mc.Priority == priority.Value);
        if (assignedToUserId.HasValue)
            query = query.Where(mc => mc.AssignedToUserId == assignedToUserId.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(mc => mc.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(mc => new ModerationCaseResponse
            {
                Id = mc.Id,
                ReportId = mc.ReportId,
                TargetType = mc.TargetType,
                TargetId = mc.TargetId,
                AssignedToUserId = mc.AssignedToUserId,
                Status = mc.Status,
                Priority = mc.Priority,
                StartedAt = mc.StartedAt,
                ResolvedAt = mc.ResolvedAt,
                ResolutionType = mc.ResolutionType,
                ResolutionNote = mc.ResolutionNote,
                CreatedAt = mc.CreatedAt,
                UpdatedAt = mc.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ModerationCaseResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ModerationCaseResponse?> GetCaseByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var caseEntity = await _cases.GetByIdAsync(id, cancellationToken);
        if (caseEntity is null) return null;

        return new ModerationCaseResponse
        {
            Id = caseEntity.Id,
            ReportId = caseEntity.ReportId,
            TargetType = caseEntity.TargetType,
            TargetId = caseEntity.TargetId,
            AssignedToUserId = caseEntity.AssignedToUserId,
            Status = caseEntity.Status,
            Priority = caseEntity.Priority,
            StartedAt = caseEntity.StartedAt,
            ResolvedAt = caseEntity.ResolvedAt,
            ResolutionType = caseEntity.ResolutionType,
            ResolutionNote = caseEntity.ResolutionNote,
            CreatedAt = caseEntity.CreatedAt,
            UpdatedAt = caseEntity.UpdatedAt
        };
    }

    public async Task<ModerationCaseResponse> CreateCaseAsync(CreateModerationCaseRequest request, CancellationToken cancellationToken = default)
    {
        var caseEntity = new ModerationCase
        {
            ReportId = request.ReportId,
            TargetType = request.TargetType,
            TargetId = request.TargetId,
            Status = ModerationCaseStatus.Open,
            Priority = request.Priority
        };

        await _cases.AddAsync(caseEntity, cancellationToken);

        return new ModerationCaseResponse
        {
            Id = caseEntity.Id,
            ReportId = caseEntity.ReportId,
            TargetType = caseEntity.TargetType,
            TargetId = caseEntity.TargetId,
            Status = caseEntity.Status,
            Priority = caseEntity.Priority,
            CreatedAt = caseEntity.CreatedAt
        };
    }

    public async Task<ModerationCaseResponse> AssignCaseAsync(Guid id, Guid assignedToUserId, CancellationToken cancellationToken = default)
    {
        var caseEntity = await _cases.GetByIdAsync(id, cancellationToken);
        if (caseEntity is null) throw new ModerationCaseNotFoundException(id);

        if (caseEntity.Status == ModerationCaseStatus.Resolved || caseEntity.Status == ModerationCaseStatus.Closed)
            throw new InvalidModerationTransitionException($"Cannot assign a case with status {caseEntity.Status}.");

        caseEntity.AssignedToUserId = assignedToUserId;
        caseEntity.Status = ModerationCaseStatus.InProgress;
        caseEntity.StartedAt = DateTime.UtcNow;
        await _cases.UpdateAsync(caseEntity, cancellationToken);

        return new ModerationCaseResponse
        {
            Id = caseEntity.Id,
            ReportId = caseEntity.ReportId,
            TargetType = caseEntity.TargetType,
            TargetId = caseEntity.TargetId,
            AssignedToUserId = caseEntity.AssignedToUserId,
            Status = caseEntity.Status,
            Priority = caseEntity.Priority,
            StartedAt = caseEntity.StartedAt,
            CreatedAt = caseEntity.CreatedAt,
            UpdatedAt = caseEntity.UpdatedAt
        };
    }

    public async Task<ModerationCaseResponse> PerformActionAsync(Guid id, ModerationActionRequest request, Guid performedByUserId, CancellationToken cancellationToken = default)
    {
        var caseEntity = await _cases.GetByIdAsync(id, cancellationToken);
        if (caseEntity is null) throw new ModerationCaseNotFoundException(id);

        if (caseEntity.Status == ModerationCaseStatus.Resolved || caseEntity.Status == ModerationCaseStatus.Closed)
            throw new InvalidModerationTransitionException($"Cannot perform action on a case with status {caseEntity.Status}.");

        var action = new ModerationAction
        {
            ModerationCaseId = caseEntity.Id,
            ActionType = request.ActionType,
            TargetType = caseEntity.TargetType,
            TargetId = caseEntity.TargetId,
            PerformedByUserId = performedByUserId,
            Reason = request.Reason,
            Notes = request.Notes
        };

        await _actions.AddAsync(action, cancellationToken);

        var history = new ModerationHistory
        {
            ModerationCaseId = caseEntity.Id,
            OldStatus = caseEntity.Status,
            NewStatus = caseEntity.Status,
            Action = request.ActionType,
            ChangedByUserId = performedByUserId,
            Reason = request.Reason,
            ChangedAt = DateTime.UtcNow
        };

        await _history.AddAsync(history, cancellationToken);

        return new ModerationCaseResponse
        {
            Id = caseEntity.Id,
            ReportId = caseEntity.ReportId,
            TargetType = caseEntity.TargetType,
            TargetId = caseEntity.TargetId,
            AssignedToUserId = caseEntity.AssignedToUserId,
            Status = caseEntity.Status,
            Priority = caseEntity.Priority,
            StartedAt = caseEntity.StartedAt,
            CreatedAt = caseEntity.CreatedAt,
            UpdatedAt = caseEntity.UpdatedAt
        };
    }

    public async Task<ModerationCaseResponse> ResolveCaseAsync(Guid id, ResolveModerationCaseRequest request, CancellationToken cancellationToken = default)
    {
        var caseEntity = await _cases.GetByIdAsync(id, cancellationToken);
        if (caseEntity is null) throw new ModerationCaseNotFoundException(id);

        if (caseEntity.Status == ModerationCaseStatus.Resolved || caseEntity.Status == ModerationCaseStatus.Closed)
            throw new InvalidModerationTransitionException($"Cannot resolve a case with status {caseEntity.Status}.");

        var oldStatus = caseEntity.Status;
        caseEntity.Status = ModerationCaseStatus.Resolved;
        caseEntity.ResolvedAt = DateTime.UtcNow;
        caseEntity.ResolutionType = request.ResolutionType;
        caseEntity.ResolutionNote = request.ResolutionNote;

        var history = new ModerationHistory
        {
            ModerationCaseId = caseEntity.Id,
            OldStatus = oldStatus,
            NewStatus = ModerationCaseStatus.Resolved,
            Action = ModerationActionType.Approve,
            ChangedByUserId = caseEntity.AssignedToUserId ?? Guid.Empty,
            Reason = request.ResolutionNote ?? "Case resolved",
            ChangedAt = DateTime.UtcNow
        };

        await _history.AddAsync(history, cancellationToken);
        await _cases.UpdateAsync(caseEntity, cancellationToken);

        return new ModerationCaseResponse
        {
            Id = caseEntity.Id,
            ReportId = caseEntity.ReportId,
            TargetType = caseEntity.TargetType,
            TargetId = caseEntity.TargetId,
            AssignedToUserId = caseEntity.AssignedToUserId,
            Status = caseEntity.Status,
            Priority = caseEntity.Priority,
            StartedAt = caseEntity.StartedAt,
            ResolvedAt = caseEntity.ResolvedAt,
            ResolutionType = caseEntity.ResolutionType,
            ResolutionNote = caseEntity.ResolutionNote,
            CreatedAt = caseEntity.CreatedAt,
            UpdatedAt = caseEntity.UpdatedAt
        };
    }

    public async Task<UserRestrictionResponse> CreateUserRestrictionAsync(CreateUserRestrictionRequest request, Guid createdByUserId, CancellationToken cancellationToken = default)
    {
        var restriction = new UserRestriction
        {
            UserId = request.UserId,
            RestrictionType = request.RestrictionType,
            Reason = request.Reason,
            StartsAt = DateTime.UtcNow,
            EndsAt = request.EndsAt,
            IsActive = true,
            CreatedByUserId = createdByUserId
        };

        await _restrictions.AddAsync(restriction, cancellationToken);

        return new UserRestrictionResponse
        {
            Id = restriction.Id,
            UserId = restriction.UserId,
            RestrictionType = restriction.RestrictionType,
            Reason = restriction.Reason,
            StartsAt = restriction.StartsAt,
            EndsAt = restriction.EndsAt,
            IsActive = restriction.IsActive,
            CreatedByUserId = restriction.CreatedByUserId,
            CreatedAt = restriction.CreatedAt
        };
    }

    public async Task<PagedResult<UserRestrictionResponse>> GetUserRestrictionsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _restrictions.QueryAsync(true, cancellationToken);
        query = query.Where(ur => ur.UserId == userId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(ur => ur.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ur => new UserRestrictionResponse
            {
                Id = ur.Id,
                UserId = ur.UserId,
                RestrictionType = ur.RestrictionType,
                Reason = ur.Reason,
                StartsAt = ur.StartsAt,
                EndsAt = ur.EndsAt,
                IsActive = ur.IsActive,
                CreatedByUserId = ur.CreatedByUserId,
                CreatedAt = ur.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<UserRestrictionResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<UserRestrictionResponse> DeactivateUserRestrictionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var restriction = await _restrictions.GetByIdAsync(id, cancellationToken);
        if (restriction is null) throw new UserRestrictionNotFoundException(id);

        restriction.IsActive = false;
        await _restrictions.UpdateAsync(restriction, cancellationToken);

        return new UserRestrictionResponse
        {
            Id = restriction.Id,
            UserId = restriction.UserId,
            RestrictionType = restriction.RestrictionType,
            Reason = restriction.Reason,
            StartsAt = restriction.StartsAt,
            EndsAt = restriction.EndsAt,
            IsActive = restriction.IsActive,
            CreatedByUserId = restriction.CreatedByUserId,
            CreatedAt = restriction.CreatedAt
        };
    }

    public async Task<PagedResult<ListingModerationResponse>> GetPendingListingsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _listings.QueryAsync(true, cancellationToken);
        query = query.Where(l => l.Status == ListingStatus.PendingReview);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new ListingModerationResponse
            {
                Id = l.Id,
                Title = l.Title,
                Price = l.Price,
                Status = l.Status,
                CreatedAt = l.CreatedAt,
                PublishedAt = l.PublishedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ListingModerationResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ListingModerationResponse> ApproveListingAsync(Guid id, Guid moderatorUserId, CancellationToken cancellationToken = default)
    {
        var listing = await _listings.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        if (listing.Status != ListingStatus.PendingReview)
            throw new InvalidModerationTransitionException($"Cannot approve a listing with status {listing.Status}.");

        listing.Status = ListingStatus.Published;
        listing.PublishedAt = DateTime.UtcNow;
        await _listings.UpdateAsync(listing, cancellationToken);

        return new ListingModerationResponse
        {
            Id = listing.Id,
            Title = listing.Title,
            Price = listing.Price,
            Status = listing.Status,
            CreatedAt = listing.CreatedAt,
            PublishedAt = listing.PublishedAt
        };
    }

    public async Task<ListingModerationResponse> RejectListingAsync(Guid id, Guid moderatorUserId, string reason, CancellationToken cancellationToken = default)
    {
        var listing = await _listings.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        if (listing.Status != ListingStatus.PendingReview)
            throw new InvalidModerationTransitionException($"Cannot reject a listing with status {listing.Status}.");

        listing.Status = ListingStatus.Rejected;
        await _listings.UpdateAsync(listing, cancellationToken);

        return new ListingModerationResponse
        {
            Id = listing.Id,
            Title = listing.Title,
            Price = listing.Price,
            Status = listing.Status,
            CreatedAt = listing.CreatedAt
        };
    }

    public async Task<ListingModerationResponse> HideListingAsync(Guid id, Guid moderatorUserId, string reason, CancellationToken cancellationToken = default)
    {
        var listing = await _listings.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        if (listing.Status != ListingStatus.Published)
            throw new InvalidModerationTransitionException($"Cannot hide a listing with status {listing.Status}.");

        listing.Status = ListingStatus.Paused;
        await _listings.UpdateAsync(listing, cancellationToken);

        return new ListingModerationResponse
        {
            Id = listing.Id,
            Title = listing.Title,
            Price = listing.Price,
            Status = listing.Status,
            CreatedAt = listing.CreatedAt
        };
    }

    public async Task<ListingModerationResponse> RestoreListingAsync(Guid id, Guid moderatorUserId, CancellationToken cancellationToken = default)
    {
        var listing = await _listings.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        if (listing.Status != ListingStatus.Paused && listing.Status != ListingStatus.Rejected)
            throw new InvalidModerationTransitionException($"Cannot restore a listing with status {listing.Status}.");

        listing.Status = ListingStatus.Published;
        listing.PublishedAt = DateTime.UtcNow;
        await _listings.UpdateAsync(listing, cancellationToken);

        return new ListingModerationResponse
        {
            Id = listing.Id,
            Title = listing.Title,
            Price = listing.Price,
            Status = listing.Status,
            CreatedAt = listing.CreatedAt,
            PublishedAt = listing.PublishedAt
        };
    }

    public async Task<IReadOnlyList<ReportReasonResponse>> GetReportReasonsAsync(TargetType? targetType, CancellationToken cancellationToken = default)
    {
        var query = await _reportReasons.QueryAsync(true, cancellationToken);
        query = query.Where(rr => rr.IsActive);

        if (targetType.HasValue)
            query = query.Where(rr => rr.TargetType == targetType.Value);

        return await query
            .OrderBy(rr => rr.DisplayOrder)
            .Select(rr => new ReportReasonResponse
            {
                Id = rr.Id,
                Code = rr.Code,
                Name = rr.Name,
                Description = rr.Description,
                TargetType = rr.TargetType,
                IsActive = rr.IsActive,
                DisplayOrder = rr.DisplayOrder
            })
            .ToListAsync(cancellationToken);
    }
}
