using VahanX.Application.Common;
using VahanX.Application.DTOs.Trust;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for inspection operations.
/// </summary>
public interface IInspectionService
{
    Task<PagedResult<InspectionResponse>> GetAllAsync(int page, int pageSize, Guid? vehicleId = null, Guid? listingId = null, Domain.Enums.InspectionStatus? status = null, CancellationToken cancellationToken = default);
    Task<InspectionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<InspectionResponse> CreateAsync(CreateInspectionRequest request, CancellationToken cancellationToken = default);
    Task<InspectionResponse> UpdateAsync(Guid id, UpdateInspectionRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<InspectionResponse> StartAsync(Guid id, CancellationToken cancellationToken = default);
    Task<InspectionResponse> CompleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<InspectionResponse> CancelAsync(Guid id, CancellationToken cancellationToken = default);
    Task<InspectionItemResponse> AddItemAsync(Guid inspectionId, CreateInspectionItemRequest request, CancellationToken cancellationToken = default);
    Task<InspectionItemResponse> UpdateItemAsync(Guid inspectionId, Guid itemId, UpdateInspectionItemRequest request, CancellationToken cancellationToken = default);
    Task DeleteItemAsync(Guid inspectionId, Guid itemId, CancellationToken cancellationToken = default);
    Task<PagedResult<InspectionItemResponse>> GetItemsAsync(Guid inspectionId, int page, int pageSize, CancellationToken cancellationToken = default);
}
