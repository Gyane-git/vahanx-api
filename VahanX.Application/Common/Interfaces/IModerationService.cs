using VahanX.Application.Common;
using VahanX.Application.DTOs.Moderation;
using VahanX.Domain.Enums;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for moderation operations.
/// </summary>
public interface IModerationService
{
    // Reports
    Task<PagedResult<ReportResponse>> GetReportsAsync(ModerationReportStatus? status, ReportPriority? priority, Guid? assignedToUserId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ReportResponse?> GetReportByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ReportResponse> CreateReportAsync(CreateReportRequest request, Guid reporterUserId, CancellationToken cancellationToken = default);
    Task<ReportResponse> AssignReportAsync(Guid id, AssignReportRequest request, CancellationToken cancellationToken = default);
    Task<ReportResponse> ResolveReportAsync(Guid id, ResolveReportRequest request, CancellationToken cancellationToken = default);
    Task<ReportResponse> RejectReportAsync(Guid id, string reason, CancellationToken cancellationToken = default);
    Task<ReportResponse> EscalateReportAsync(Guid id, CancellationToken cancellationToken = default);

    // Cases
    Task<PagedResult<ModerationCaseResponse>> GetCasesAsync(ModerationCaseStatus? status, ReportPriority? priority, Guid? assignedToUserId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ModerationCaseResponse?> GetCaseByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ModerationCaseResponse> CreateCaseAsync(CreateModerationCaseRequest request, CancellationToken cancellationToken = default);
    Task<ModerationCaseResponse> AssignCaseAsync(Guid id, Guid assignedToUserId, CancellationToken cancellationToken = default);
    Task<ModerationCaseResponse> PerformActionAsync(Guid id, ModerationActionRequest request, Guid performedByUserId, CancellationToken cancellationToken = default);
    Task<ModerationCaseResponse> ResolveCaseAsync(Guid id, ResolveModerationCaseRequest request, CancellationToken cancellationToken = default);

    // User Restrictions
    Task<UserRestrictionResponse> CreateUserRestrictionAsync(CreateUserRestrictionRequest request, Guid createdByUserId, CancellationToken cancellationToken = default);
    Task<PagedResult<UserRestrictionResponse>> GetUserRestrictionsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<UserRestrictionResponse> DeactivateUserRestrictionAsync(Guid id, CancellationToken cancellationToken = default);

    // Listing Moderation
    Task<PagedResult<ListingModerationResponse>> GetPendingListingsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ListingModerationResponse> ApproveListingAsync(Guid id, Guid moderatorUserId, CancellationToken cancellationToken = default);
    Task<ListingModerationResponse> RejectListingAsync(Guid id, Guid moderatorUserId, string reason, CancellationToken cancellationToken = default);
    Task<ListingModerationResponse> HideListingAsync(Guid id, Guid moderatorUserId, string reason, CancellationToken cancellationToken = default);
    Task<ListingModerationResponse> RestoreListingAsync(Guid id, Guid moderatorUserId, CancellationToken cancellationToken = default);

    // Report Reasons
    Task<IReadOnlyList<ReportReasonResponse>> GetReportReasonsAsync(TargetType? targetType, CancellationToken cancellationToken = default);
}

/// <summary>
/// Response DTO for listing moderation.
/// </summary>
public class ListingModerationResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SellerName { get; set; }
    public string? DealerName { get; set; }
    public decimal Price { get; set; }
    public ListingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
}
