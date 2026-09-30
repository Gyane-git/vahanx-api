using VahanX.Application.Common;
using VahanX.Application.DTOs.Trust;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for verification operations.
/// </summary>
public interface IVerificationService
{
    Task<PagedResult<VerificationResponse>> GetAllAsync(int page, int pageSize, Guid? vehicleId = null, Guid? listingId = null, Domain.Enums.VehicleVerificationStatus? status = null, CancellationToken cancellationToken = default);
    Task<VerificationResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PublicVerificationResponse?> GetPublicByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);
    Task<VerificationResponse> CreateAsync(CreateVerificationRequest request, CancellationToken cancellationToken = default);
    Task<VerificationResponse> UpdateAsync(Guid id, UpdateVerificationRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VerificationResponse> SubmitAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VerificationResponse> ApproveAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VerificationResponse> RejectAsync(Guid id, string reason, CancellationToken cancellationToken = default);
    Task<VerificationResponse> RevokeAsync(Guid id, string reason, CancellationToken cancellationToken = default);
}
