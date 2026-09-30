using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Business;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of advertisement service.
/// </summary>
public class AdvertisementService : IAdvertisementService
{
    private readonly IRepository<AdvertisementCampaign> _campaignRepository;
    private readonly IRepository<Advertisement> _advertisementRepository;

    public AdvertisementService(
        IRepository<AdvertisementCampaign> campaignRepository,
        IRepository<Advertisement> advertisementRepository)
    {
        _campaignRepository = campaignRepository;
        _advertisementRepository = advertisementRepository;
    }

    public async Task<PagedResult<AdvertisementCampaignResponse>> GetCampaignsAsync(int page, int pageSize, Guid? userId = null, AdvertisementCampaignStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = await _campaignRepository.QueryAsync(true, cancellationToken);

        if (userId.HasValue)
            query = query.Where(c => c.AdvertiserUserId == userId.Value);
        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new AdvertisementCampaignResponse
            {
                Id = c.Id,
                AdvertiserUserId = c.AdvertiserUserId,
                SellerId = c.SellerId,
                DealerId = c.DealerId,
                Name = c.Name,
                Description = c.Description,
                Status = c.Status,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                BudgetType = c.BudgetType,
                BudgetAmount = c.BudgetAmount,
                Currency = c.Currency
            })
            .ToListAsync(cancellationToken);

        return PagedResult<AdvertisementCampaignResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<AdvertisementCampaignResponse?> GetCampaignByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id, cancellationToken);
        if (campaign is null) return null;

        if (campaign.AdvertiserUserId != userId)
            throw new ForbiddenException("You do not have access to this campaign.");

        return new AdvertisementCampaignResponse
        {
            Id = campaign.Id,
            AdvertiserUserId = campaign.AdvertiserUserId,
            SellerId = campaign.SellerId,
            DealerId = campaign.DealerId,
            Name = campaign.Name,
            Description = campaign.Description,
            Status = campaign.Status,
            StartDate = campaign.StartDate,
            EndDate = campaign.EndDate,
            BudgetType = campaign.BudgetType,
            BudgetAmount = campaign.BudgetAmount,
            Currency = campaign.Currency
        };
    }

    public async Task<AdvertisementCampaignResponse> CreateCampaignAsync(CreateAdvertisementCampaignRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var campaign = new AdvertisementCampaign
        {
            AdvertiserUserId = userId,
            Name = request.Name,
            Description = request.Description,
            Status = AdvertisementCampaignStatus.Draft,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            BudgetType = request.BudgetType,
            BudgetAmount = request.BudgetAmount,
            DailyBudget = request.DailyBudget,
            TotalBudget = request.TotalBudget,
            Currency = request.Currency
        };

        await _campaignRepository.AddAsync(campaign, cancellationToken);

        return new AdvertisementCampaignResponse
        {
            Id = campaign.Id,
            AdvertiserUserId = campaign.AdvertiserUserId,
            SellerId = campaign.SellerId,
            DealerId = campaign.DealerId,
            Name = campaign.Name,
            Description = campaign.Description,
            Status = campaign.Status,
            StartDate = campaign.StartDate,
            EndDate = campaign.EndDate,
            BudgetType = campaign.BudgetType,
            BudgetAmount = campaign.BudgetAmount,
            Currency = campaign.Currency
        };
    }

    public async Task<AdvertisementCampaignResponse> UpdateCampaignAsync(Guid id, UpdateAdvertisementCampaignRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id, cancellationToken);
        if (campaign is null) throw new NotFoundException("AdvertisementCampaign", id);

        if (campaign.AdvertiserUserId != userId)
            throw new ForbiddenException("You can only update your own campaigns.");

        if (campaign.Status != AdvertisementCampaignStatus.Draft && campaign.Status != AdvertisementCampaignStatus.Paused)
            throw new ConflictException($"Cannot update a campaign with status {campaign.Status}.");

        if (request.Name != null) campaign.Name = request.Name;
        if (request.Description != null) campaign.Description = request.Description;
        if (request.StartDate.HasValue) campaign.StartDate = request.StartDate.Value;
        if (request.EndDate.HasValue) campaign.EndDate = request.EndDate.Value;
        if (request.BudgetAmount.HasValue) campaign.BudgetAmount = request.BudgetAmount.Value;

        await _campaignRepository.UpdateAsync(campaign, cancellationToken);

        return new AdvertisementCampaignResponse
        {
            Id = campaign.Id,
            AdvertiserUserId = campaign.AdvertiserUserId,
            SellerId = campaign.SellerId,
            DealerId = campaign.DealerId,
            Name = campaign.Name,
            Description = campaign.Description,
            Status = campaign.Status,
            StartDate = campaign.StartDate,
            EndDate = campaign.EndDate,
            BudgetType = campaign.BudgetType,
            BudgetAmount = campaign.BudgetAmount,
            Currency = campaign.Currency
        };
    }

    public async Task<AdvertisementCampaignResponse> SubmitCampaignAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id, cancellationToken);
        if (campaign is null) throw new NotFoundException("AdvertisementCampaign", id);

        if (campaign.AdvertiserUserId != userId)
            throw new ForbiddenException("You can only submit your own campaigns.");

        if (campaign.Status != AdvertisementCampaignStatus.Draft)
            throw new ConflictException($"Cannot submit a campaign with status {campaign.Status}.");

        campaign.Status = AdvertisementCampaignStatus.PendingReview;
        await _campaignRepository.UpdateAsync(campaign, cancellationToken);

        return new AdvertisementCampaignResponse
        {
            Id = campaign.Id,
            AdvertiserUserId = campaign.AdvertiserUserId,
            SellerId = campaign.SellerId,
            DealerId = campaign.DealerId,
            Name = campaign.Name,
            Description = campaign.Description,
            Status = campaign.Status,
            StartDate = campaign.StartDate,
            EndDate = campaign.EndDate,
            BudgetType = campaign.BudgetType,
            BudgetAmount = campaign.BudgetAmount,
            Currency = campaign.Currency
        };
    }

    public async Task<AdvertisementCampaignResponse> PauseCampaignAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id, cancellationToken);
        if (campaign is null) throw new NotFoundException("AdvertisementCampaign", id);

        if (campaign.AdvertiserUserId != userId)
            throw new ForbiddenException("You can only pause your own campaigns.");

        if (campaign.Status != AdvertisementCampaignStatus.Running)
            throw new ConflictException($"Cannot pause a campaign with status {campaign.Status}.");

        campaign.Status = AdvertisementCampaignStatus.Paused;
        await _campaignRepository.UpdateAsync(campaign, cancellationToken);

        return new AdvertisementCampaignResponse
        {
            Id = campaign.Id,
            AdvertiserUserId = campaign.AdvertiserUserId,
            SellerId = campaign.SellerId,
            DealerId = campaign.DealerId,
            Name = campaign.Name,
            Description = campaign.Description,
            Status = campaign.Status,
            StartDate = campaign.StartDate,
            EndDate = campaign.EndDate,
            BudgetType = campaign.BudgetType,
            BudgetAmount = campaign.BudgetAmount,
            Currency = campaign.Currency
        };
    }

    public async Task<AdvertisementCampaignResponse> ResumeCampaignAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id, cancellationToken);
        if (campaign is null) throw new NotFoundException("AdvertisementCampaign", id);

        if (campaign.AdvertiserUserId != userId)
            throw new ForbiddenException("You can only resume your own campaigns.");

        if (campaign.Status != AdvertisementCampaignStatus.Paused)
            throw new ConflictException($"Cannot resume a campaign with status {campaign.Status}.");

        campaign.Status = AdvertisementCampaignStatus.Running;
        await _campaignRepository.UpdateAsync(campaign, cancellationToken);

        return new AdvertisementCampaignResponse
        {
            Id = campaign.Id,
            AdvertiserUserId = campaign.AdvertiserUserId,
            SellerId = campaign.SellerId,
            DealerId = campaign.DealerId,
            Name = campaign.Name,
            Description = campaign.Description,
            Status = campaign.Status,
            StartDate = campaign.StartDate,
            EndDate = campaign.EndDate,
            BudgetType = campaign.BudgetType,
            BudgetAmount = campaign.BudgetAmount,
            Currency = campaign.Currency
        };
    }

    public async Task<AdvertisementCampaignResponse> CancelCampaignAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id, cancellationToken);
        if (campaign is null) throw new NotFoundException("AdvertisementCampaign", id);

        if (campaign.AdvertiserUserId != userId)
            throw new ForbiddenException("You can only cancel your own campaigns.");

        if (campaign.Status == AdvertisementCampaignStatus.Completed || campaign.Status == AdvertisementCampaignStatus.Cancelled)
            throw new ConflictException($"Cannot cancel a campaign with status {campaign.Status}.");

        campaign.Status = AdvertisementCampaignStatus.Cancelled;
        await _campaignRepository.UpdateAsync(campaign, cancellationToken);

        return new AdvertisementCampaignResponse
        {
            Id = campaign.Id,
            AdvertiserUserId = campaign.AdvertiserUserId,
            SellerId = campaign.SellerId,
            DealerId = campaign.DealerId,
            Name = campaign.Name,
            Description = campaign.Description,
            Status = campaign.Status,
            StartDate = campaign.StartDate,
            EndDate = campaign.EndDate,
            BudgetType = campaign.BudgetType,
            BudgetAmount = campaign.BudgetAmount,
            Currency = campaign.Currency
        };
    }

    public async Task<AdvertisementResponse> CreateAdvertisementAsync(CreateAdvertisementRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(request.CampaignId, cancellationToken);
        if (campaign is null) throw new NotFoundException("AdvertisementCampaign", request.CampaignId);

        if (campaign.AdvertiserUserId != userId)
            throw new ForbiddenException("You can only add advertisements to your own campaigns.");

        var advertisement = new Advertisement
        {
            CampaignId = request.CampaignId,
            Name = request.Name,
            Headline = request.Headline,
            Description = request.Description,
            DestinationType = request.DestinationType,
            DestinationUrl = request.DestinationUrl,
            VehicleListingId = request.VehicleListingId,
            MediaReference = request.MediaReference,
            Status = AdvertisementStatus.Draft
        };

        await _advertisementRepository.AddAsync(advertisement, cancellationToken);

        return new AdvertisementResponse
        {
            Id = advertisement.Id,
            CampaignId = advertisement.CampaignId,
            Name = advertisement.Name,
            Headline = advertisement.Headline,
            Description = advertisement.Description,
            DestinationType = advertisement.DestinationType,
            DestinationUrl = advertisement.DestinationUrl,
            VehicleListingId = advertisement.VehicleListingId,
            MediaReference = advertisement.MediaReference,
            Status = advertisement.Status
        };
    }

    public async Task<AdvertisementResponse> UpdateAdvertisementAsync(Guid id, UpdateAdvertisementRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var advertisement = await _advertisementRepository.GetByIdAsync(id, cancellationToken);
        if (advertisement is null) throw new NotFoundException("Advertisement", id);

        var campaign = await _campaignRepository.GetByIdAsync(advertisement.CampaignId, cancellationToken);
        if (campaign == null) throw new NotFoundException("AdvertisementCampaign", advertisement.CampaignId);

        if (campaign.AdvertiserUserId != userId)
            throw new ForbiddenException("You can only update your own advertisements.");

        if (request.Name != null) advertisement.Name = request.Name;
        if (request.Headline != null) advertisement.Headline = request.Headline;
        if (request.Description != null) advertisement.Description = request.Description;
        if (request.DestinationUrl != null) advertisement.DestinationUrl = request.DestinationUrl;
        if (request.MediaReference != null) advertisement.MediaReference = request.MediaReference;

        await _advertisementRepository.UpdateAsync(advertisement, cancellationToken);

        return new AdvertisementResponse
        {
            Id = advertisement.Id,
            CampaignId = advertisement.CampaignId,
            Name = advertisement.Name,
            Headline = advertisement.Headline,
            Description = advertisement.Description,
            DestinationType = advertisement.DestinationType,
            DestinationUrl = advertisement.DestinationUrl,
            VehicleListingId = advertisement.VehicleListingId,
            MediaReference = advertisement.MediaReference,
            Status = advertisement.Status
        };
    }

    public async Task<AdvertisementResponse?> GetAdvertisementByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var advertisement = await _advertisementRepository.GetByIdAsync(id, cancellationToken);
        if (advertisement is null) return null;

        return new AdvertisementResponse
        {
            Id = advertisement.Id,
            CampaignId = advertisement.CampaignId,
            Name = advertisement.Name,
            Headline = advertisement.Headline,
            Description = advertisement.Description,
            DestinationType = advertisement.DestinationType,
            DestinationUrl = advertisement.DestinationUrl,
            VehicleListingId = advertisement.VehicleListingId,
            MediaReference = advertisement.MediaReference,
            Status = advertisement.Status
        };
    }

    public async Task<PagedResult<AdvertisementResponse>> GetCampaignAdvertisementsAsync(Guid campaignId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _advertisementRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(a => a.CampaignId == campaignId).OrderBy(a => a.Name);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AdvertisementResponse
            {
                Id = a.Id,
                CampaignId = a.CampaignId,
                Name = a.Name,
                Headline = a.Headline,
                Description = a.Description,
                DestinationType = a.DestinationType,
                DestinationUrl = a.DestinationUrl,
                VehicleListingId = a.VehicleListingId,
                MediaReference = a.MediaReference,
                Status = a.Status
            })
            .ToListAsync(cancellationToken);

        return PagedResult<AdvertisementResponse>.Create(items, totalCount, page, pageSize);
    }
}
