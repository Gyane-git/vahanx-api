using VahanX.Application.Common;
using VahanX.Application.DTOs.Business;
using VahanX.Domain.Enums;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for advertisement operations.
/// </summary>
public interface IAdvertisementService
{
    Task<PagedResult<AdvertisementCampaignResponse>> GetCampaignsAsync(int page, int pageSize, Guid? userId = null, AdvertisementCampaignStatus? status = null, CancellationToken cancellationToken = default);
    Task<AdvertisementCampaignResponse?> GetCampaignByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<AdvertisementCampaignResponse> CreateCampaignAsync(CreateAdvertisementCampaignRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<AdvertisementCampaignResponse> UpdateCampaignAsync(Guid id, UpdateAdvertisementCampaignRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<AdvertisementCampaignResponse> SubmitCampaignAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<AdvertisementCampaignResponse> PauseCampaignAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<AdvertisementCampaignResponse> ResumeCampaignAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<AdvertisementCampaignResponse> CancelCampaignAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<AdvertisementResponse> CreateAdvertisementAsync(CreateAdvertisementRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<AdvertisementResponse> UpdateAdvertisementAsync(Guid id, UpdateAdvertisementRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<AdvertisementResponse?> GetAdvertisementByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<AdvertisementResponse>> GetCampaignAdvertisementsAsync(Guid campaignId, int page, int pageSize, CancellationToken cancellationToken = default);
}
