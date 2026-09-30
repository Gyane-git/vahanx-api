using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of dealer service.
/// </summary>
public class DealerService : IDealerService
{
    private readonly IRepository<Dealer> _repository;
    private readonly IRepository<DealerBranch> _branchRepository;
    private readonly IRepository<VehicleListing> _listingRepository;

    public DealerService(
        IRepository<Dealer> repository,
        IRepository<DealerBranch> branchRepository,
        IRepository<VehicleListing> listingRepository)
    {
        _repository = repository;
        _branchRepository = branchRepository;
        _listingRepository = listingRepository;
    }

    public async Task<PagedResult<DealerResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DealerResponse
            {
                Id = d.Id,
                BusinessName = d.BusinessName,
                LegalName = d.LegalName,
                RegistrationNumber = d.RegistrationNumber,
                Description = d.Description,
                Phone = d.Phone,
                Email = d.Email,
                Website = d.Website,
                LocationId = d.LocationId,
                LocationName = d.Location != null ? d.Location.Name : null,
                VerificationStatus = d.VerificationStatus,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<DealerResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<DealerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dealer = await _repository.GetByIdAsync(id, cancellationToken);

        if (dealer is null)
            return null;

        return new DealerResponse
        {
            Id = dealer.Id,
            BusinessName = dealer.BusinessName,
            LegalName = dealer.LegalName,
            RegistrationNumber = dealer.RegistrationNumber,
            Description = dealer.Description,
            Phone = dealer.Phone,
            Email = dealer.Email,
            Website = dealer.Website,
            LocationId = dealer.LocationId,
            LocationName = dealer.Location != null ? dealer.Location.Name : null,
            VerificationStatus = dealer.VerificationStatus,
            IsActive = dealer.IsActive,
            CreatedAt = dealer.CreatedAt,
            UpdatedAt = dealer.UpdatedAt
        };
    }

    public async Task<DealerResponse> CreateAsync(CreateDealerRequest request, CancellationToken cancellationToken = default)
    {
        var dealer = new Dealer
        {
            BusinessName = request.BusinessName,
            LegalName = request.LegalName,
            RegistrationNumber = request.RegistrationNumber,
            Description = request.Description,
            Phone = request.Phone,
            Email = request.Email,
            Website = request.Website,
            LocationId = request.LocationId
        };

        await _repository.AddAsync(dealer, cancellationToken);

        return new DealerResponse
        {
            Id = dealer.Id,
            BusinessName = dealer.BusinessName,
            LegalName = dealer.LegalName,
            RegistrationNumber = dealer.RegistrationNumber,
            Description = dealer.Description,
            Phone = dealer.Phone,
            Email = dealer.Email,
            Website = dealer.Website,
            LocationId = dealer.LocationId,
            LocationName = null,
            VerificationStatus = dealer.VerificationStatus,
            IsActive = dealer.IsActive,
            CreatedAt = dealer.CreatedAt,
            UpdatedAt = dealer.UpdatedAt
        };
    }

    public async Task<DealerResponse> UpdateAsync(Guid id, UpdateDealerRequest request, CancellationToken cancellationToken = default)
    {
        var dealer = await _repository.GetByIdAsync(id, cancellationToken);
        if (dealer is null) throw new NotFoundException("Dealer", id);

        dealer.BusinessName = request.BusinessName;
        dealer.LegalName = request.LegalName;
        dealer.RegistrationNumber = request.RegistrationNumber;
        dealer.Description = request.Description;
        dealer.Phone = request.Phone;
        dealer.Email = request.Email;
        dealer.Website = request.Website;
        dealer.LocationId = request.LocationId;
        dealer.IsActive = request.IsActive;

        await _repository.UpdateAsync(dealer, cancellationToken);

        return new DealerResponse
        {
            Id = dealer.Id,
            BusinessName = dealer.BusinessName,
            LegalName = dealer.LegalName,
            RegistrationNumber = dealer.RegistrationNumber,
            Description = dealer.Description,
            Phone = dealer.Phone,
            Email = dealer.Email,
            Website = dealer.Website,
            LocationId = dealer.LocationId,
            LocationName = null,
            VerificationStatus = dealer.VerificationStatus,
            IsActive = dealer.IsActive,
            CreatedAt = dealer.CreatedAt,
            UpdatedAt = dealer.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dealer = await _repository.GetByIdAsync(id, cancellationToken);
        if (dealer is null) throw new NotFoundException("Dealer", id);

        await _repository.DeleteAsync(dealer, cancellationToken);
    }

    public async Task<PagedResult<ListingResponse>> GetDealerListingsAsync(Guid dealerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _listingRepository.QueryAsync(true, cancellationToken);

        var listingsQuery = query
            .Where(l => l.DealerId == dealerId)
            .OrderByDescending(l => l.CreatedAt);

        var totalCount = await listingsQuery.CountAsync(cancellationToken);
        var items = await listingsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new ListingResponse
            {
                Id = l.Id,
                VehicleId = l.VehicleId,
                VehicleName = string.Empty,
                SellerId = l.SellerId,
                SellerName = null,
                DealerId = l.DealerId,
                DealerName = null,
                Title = l.Title,
                Description = l.Description,
                Price = l.Price,
                Currency = l.Currency,
                Mileage = l.Mileage,
                MileageUnit = l.MileageUnit,
                ManufactureYear = l.ManufactureYear,
                RegistrationYear = l.RegistrationYear,
                Condition = l.Condition,
                Status = l.Status,
                LocationId = l.LocationId,
                LocationName = null,
                Latitude = l.Latitude,
                Longitude = l.Longitude,
                ContactPreference = l.ContactPreference,
                IsNegotiable = l.IsNegotiable,
                PublishedAt = l.PublishedAt,
                ExpiresAt = l.ExpiresAt,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ListingResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<DealerBranchResponse> CreateBranchAsync(Guid dealerId, CreateDealerBranchRequest request, CancellationToken cancellationToken = default)
    {
        var dealerExists = await _repository.AnyAsync(d => d.Id == dealerId, cancellationToken);
        if (!dealerExists) throw new NotFoundException("Dealer", dealerId);

        var branch = new DealerBranch
        {
            DealerId = dealerId,
            Name = request.Name,
            Phone = request.Phone,
            Email = request.Email,
            LocationId = request.LocationId,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        await _branchRepository.AddAsync(branch, cancellationToken);

        return new DealerBranchResponse
        {
            Id = branch.Id,
            DealerId = branch.DealerId,
            DealerName = string.Empty,
            Name = branch.Name,
            Phone = branch.Phone,
            Email = branch.Email,
            LocationId = branch.LocationId,
            LocationName = null,
            Latitude = branch.Latitude,
            Longitude = branch.Longitude,
            IsActive = branch.IsActive,
            CreatedAt = branch.CreatedAt,
            UpdatedAt = branch.UpdatedAt
        };
    }

    public async Task<DealerBranchResponse> UpdateBranchAsync(Guid dealerId, Guid branchId, UpdateDealerBranchRequest request, CancellationToken cancellationToken = default)
    {
        var branch = await _branchRepository.GetByIdAsync(branchId, cancellationToken);
        if (branch is null) throw new NotFoundException("DealerBranch", branchId);

        if (branch.DealerId != dealerId)
            throw new NotFoundException("DealerBranch", branchId);

        branch.Name = request.Name;
        branch.Phone = request.Phone;
        branch.Email = request.Email;
        branch.LocationId = request.LocationId;
        branch.Latitude = request.Latitude;
        branch.Longitude = request.Longitude;
        branch.IsActive = request.IsActive;

        await _branchRepository.UpdateAsync(branch, cancellationToken);

        return new DealerBranchResponse
        {
            Id = branch.Id,
            DealerId = branch.DealerId,
            DealerName = string.Empty,
            Name = branch.Name,
            Phone = branch.Phone,
            Email = branch.Email,
            LocationId = branch.LocationId,
            LocationName = null,
            Latitude = branch.Latitude,
            Longitude = branch.Longitude,
            IsActive = branch.IsActive,
            CreatedAt = branch.CreatedAt,
            UpdatedAt = branch.UpdatedAt
        };
    }

    public async Task DeleteBranchAsync(Guid dealerId, Guid branchId, CancellationToken cancellationToken = default)
    {
        var branch = await _branchRepository.GetByIdAsync(branchId, cancellationToken);
        if (branch is null) throw new NotFoundException("DealerBranch", branchId);

        if (branch.DealerId != dealerId)
            throw new NotFoundException("DealerBranch", branchId);

        await _branchRepository.DeleteAsync(branch, cancellationToken);
    }

    public async Task<PagedResult<DealerBranchResponse>> GetBranchesAsync(Guid dealerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _branchRepository.QueryAsync(true, cancellationToken);

        var branchesQuery = query
            .Where(b => b.DealerId == dealerId)
            .OrderByDescending(b => b.CreatedAt);

        var totalCount = await branchesQuery.CountAsync(cancellationToken);
        var items = await branchesQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new DealerBranchResponse
            {
                Id = b.Id,
                DealerId = b.DealerId,
                DealerName = string.Empty,
                Name = b.Name,
                Phone = b.Phone,
                Email = b.Email,
                LocationId = b.LocationId,
                LocationName = null,
                Latitude = b.Latitude,
                Longitude = b.Longitude,
                IsActive = b.IsActive,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<DealerBranchResponse>.Create(items, totalCount, page, pageSize);
    }
}
