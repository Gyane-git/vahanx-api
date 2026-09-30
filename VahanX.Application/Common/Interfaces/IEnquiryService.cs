using VahanX.Application.Common;
using VahanX.Application.DTOs.Engagement;
using VahanX.Domain.Enums;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for enquiry operations.
/// </summary>
public interface IEnquiryService
{
    Task<PagedResult<EnquiryResponse>> GetAllAsync(int page, int pageSize, Guid? userId = null, Guid? listingId = null, Guid? sellerId = null, Guid? dealerId = null, EnquiryStatus? status = null, CancellationToken cancellationToken = default);
    Task<EnquiryResponse?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<EnquiryResponse> CreateAsync(CreateEnquiryRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<EnquiryResponse> UpdateAsync(Guid id, UpdateEnquiryRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<EnquiryResponse> RespondAsync(Guid id, RespondEnquiryRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<EnquiryResponse> CloseAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<EnquiryResponse> CancelAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<EnquiryResponse>> GetByListingAsync(Guid listingId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<EnquiryResponse>> GetBySellerAsync(Guid sellerId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<EnquiryResponse>> GetByDealerAsync(Guid dealerId, int page, int pageSize, CancellationToken cancellationToken = default);
}
