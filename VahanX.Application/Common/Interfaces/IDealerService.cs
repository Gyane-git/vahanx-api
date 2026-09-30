using VahanX.Application.Common;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for dealer operations.
/// </summary>
public interface IDealerService
{
    Task<PagedResult<DealerResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<DealerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DealerResponse> CreateAsync(CreateDealerRequest request, CancellationToken cancellationToken = default);
    Task<DealerResponse> UpdateAsync(Guid id, UpdateDealerRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<ListingResponse>> GetDealerListingsAsync(Guid dealerId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<DealerBranchResponse> CreateBranchAsync(Guid dealerId, CreateDealerBranchRequest request, CancellationToken cancellationToken = default);
    Task<DealerBranchResponse> UpdateBranchAsync(Guid dealerId, Guid branchId, UpdateDealerBranchRequest request, CancellationToken cancellationToken = default);
    Task DeleteBranchAsync(Guid dealerId, Guid branchId, CancellationToken cancellationToken = default);
    Task<PagedResult<DealerBranchResponse>> GetBranchesAsync(Guid dealerId, int page, int pageSize, CancellationToken cancellationToken = default);
}
