using VahanX.Application.Common;
using VahanX.Application.DTOs.Engagement;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for test drive operations.
/// </summary>
public interface ITestDriveService
{
    Task<PagedResult<TestDriveResponse>> GetAllAsync(int page, int pageSize, Guid? userId = null, Guid? listingId = null, Guid? dealerId = null, Domain.Enums.TestDriveStatus? status = null, CancellationToken cancellationToken = default);
    Task<TestDriveResponse?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<TestDriveResponse> CreateAsync(CreateTestDriveRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<TestDriveResponse> UpdateAsync(Guid id, UpdateTestDriveRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<TestDriveResponse> ConfirmAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<TestDriveResponse> RescheduleAsync(Guid id, RescheduleTestDriveRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<TestDriveResponse> CancelAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<TestDriveResponse> CompleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<TestDriveResponse>> GetByListingAsync(Guid listingId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<TestDriveResponse>> GetByDealerAsync(Guid dealerId, int page, int pageSize, CancellationToken cancellationToken = default);
}
