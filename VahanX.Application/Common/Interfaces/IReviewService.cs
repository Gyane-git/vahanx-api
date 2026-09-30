using VahanX.Application.Common;
using VahanX.Application.DTOs.Trust;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for review operations.
/// </summary>
public interface IReviewService
{
    Task<PagedResult<VehicleReviewResponse>> GetVehicleReviewsAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<SellerReviewResponse>> GetSellerReviewsAsync(Guid sellerId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<DealerReviewResponse>> GetDealerReviewsAsync(Guid dealerId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<VehicleReviewResponse?> GetVehicleReviewByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleReviewResponse> CreateVehicleReviewAsync(Guid vehicleId, Guid userId, CreateReviewRequest request, CancellationToken cancellationToken = default);
    Task<SellerReviewResponse> CreateSellerReviewAsync(Guid sellerId, Guid userId, CreateReviewRequest request, CancellationToken cancellationToken = default);
    Task<DealerReviewResponse> CreateDealerReviewAsync(Guid dealerId, Guid userId, CreateReviewRequest request, CancellationToken cancellationToken = default);
    Task<VehicleReviewResponse> UpdateVehicleReviewAsync(Guid id, Guid userId, UpdateReviewRequest request, CancellationToken cancellationToken = default);
    Task DeleteReviewAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task ReportReviewAsync(Guid id, Guid userId, ReportReviewRequest request, CancellationToken cancellationToken = default);
    Task<RatingSummaryResponse> GetVehicleRatingSummaryAsync(Guid vehicleId, CancellationToken cancellationToken = default);
    Task<RatingSummaryResponse> GetSellerRatingSummaryAsync(Guid sellerId, CancellationToken cancellationToken = default);
    Task<RatingSummaryResponse> GetDealerRatingSummaryAsync(Guid dealerId, CancellationToken cancellationToken = default);
}
