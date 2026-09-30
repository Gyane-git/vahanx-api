using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for compare list operations.
/// </summary>
public interface ICompareService
{
    Task<CompareResponse> GetCompareListAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CompareResponse> AddItemAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default);
    Task<CompareResponse> RemoveItemAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default);
}
