using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of service center service.
/// </summary>
public class ServiceCenterService : IServiceCenterService
{
    private readonly IRepository<ServiceCenter> _repository;
    private readonly IRepository<ServiceCenterBranch> _branchRepository;

    public ServiceCenterService(
        IRepository<ServiceCenter> repository,
        IRepository<ServiceCenterBranch> branchRepository)
    {
        _repository = repository;
        _branchRepository = branchRepository;
    }

    public async Task<PagedResult<ServiceCenterListDto>> GetAllAsync(int page, int pageSize, string? keyword = null, Guid? locationId = null, ServiceCenterStatus? status = null, VerificationStatus? verificationStatus = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(sc => sc.Name.Contains(keyword) || (sc.Description != null && sc.Description.Contains(keyword)));

        if (status.HasValue)
            query = query.Where(sc => sc.Status == status.Value);

        if (verificationStatus.HasValue)
            query = query.Where(sc => sc.VerificationStatus == verificationStatus.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(sc => sc.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(sc => new ServiceCenterListDto
            {
                Id = sc.Id,
                Name = sc.Name,
                Description = sc.Description,
                ContactPhone = sc.ContactPhone,
                ContactEmail = sc.ContactEmail,
                WebsiteUrl = sc.WebsiteUrl,
                VerificationStatus = sc.VerificationStatus,
                Status = sc.Status
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ServiceCenterListDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ServiceCenterDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var serviceCenter = await _repository.GetByIdAsync(id, cancellationToken);
        if (serviceCenter is null) return null;

        return new ServiceCenterDetailsDto
        {
            Id = serviceCenter.Id,
            Name = serviceCenter.Name,
            Description = serviceCenter.Description,
            BusinessRegistrationNumber = serviceCenter.BusinessRegistrationNumber,
            ContactPhone = serviceCenter.ContactPhone,
            ContactEmail = serviceCenter.ContactEmail,
            WebsiteUrl = serviceCenter.WebsiteUrl,
            LogoMediaReference = serviceCenter.LogoMediaReference,
            VerificationStatus = serviceCenter.VerificationStatus,
            Status = serviceCenter.Status,
            CreatedAt = serviceCenter.CreatedAt,
            UpdatedAt = serviceCenter.UpdatedAt
        };
    }

    public async Task<ServiceCenterDetailsDto> CreateAsync(CreateServiceCenterRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var serviceCenter = new ServiceCenter
        {
            Name = request.Name,
            Description = request.Description,
            OwnerUserId = userId,
            BusinessRegistrationNumber = request.BusinessRegistrationNumber,
            ContactPhone = request.ContactPhone,
            ContactEmail = request.ContactEmail,
            WebsiteUrl = request.WebsiteUrl,
            LogoMediaReference = request.LogoMediaReference,
            Status = ServiceCenterStatus.PendingApproval
        };

        await _repository.AddAsync(serviceCenter, cancellationToken);

        return new ServiceCenterDetailsDto
        {
            Id = serviceCenter.Id,
            Name = serviceCenter.Name,
            Description = serviceCenter.Description,
            BusinessRegistrationNumber = serviceCenter.BusinessRegistrationNumber,
            ContactPhone = serviceCenter.ContactPhone,
            ContactEmail = serviceCenter.ContactEmail,
            WebsiteUrl = serviceCenter.WebsiteUrl,
            LogoMediaReference = serviceCenter.LogoMediaReference,
            VerificationStatus = serviceCenter.VerificationStatus,
            Status = serviceCenter.Status,
            CreatedAt = serviceCenter.CreatedAt,
            UpdatedAt = serviceCenter.UpdatedAt
        };
    }

    public async Task<ServiceCenterDetailsDto> UpdateAsync(Guid id, UpdateServiceCenterRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var serviceCenter = await _repository.GetByIdAsync(id, cancellationToken);
        if (serviceCenter is null) throw new NotFoundException("ServiceCenter", id);

        if (serviceCenter.OwnerUserId != userId)
            throw new ForbiddenException("You can only update your own service centers.");

        if (request.Name != null) serviceCenter.Name = request.Name;
        if (request.Description != null) serviceCenter.Description = request.Description;
        if (request.BusinessRegistrationNumber != null) serviceCenter.BusinessRegistrationNumber = request.BusinessRegistrationNumber;
        if (request.ContactPhone != null) serviceCenter.ContactPhone = request.ContactPhone;
        if (request.ContactEmail != null) serviceCenter.ContactEmail = request.ContactEmail;
        if (request.WebsiteUrl != null) serviceCenter.WebsiteUrl = request.WebsiteUrl;
        if (request.LogoMediaReference != null) serviceCenter.LogoMediaReference = request.LogoMediaReference;

        await _repository.UpdateAsync(serviceCenter, cancellationToken);

        return new ServiceCenterDetailsDto
        {
            Id = serviceCenter.Id,
            Name = serviceCenter.Name,
            Description = serviceCenter.Description,
            BusinessRegistrationNumber = serviceCenter.BusinessRegistrationNumber,
            ContactPhone = serviceCenter.ContactPhone,
            ContactEmail = serviceCenter.ContactEmail,
            WebsiteUrl = serviceCenter.WebsiteUrl,
            LogoMediaReference = serviceCenter.LogoMediaReference,
            VerificationStatus = serviceCenter.VerificationStatus,
            Status = serviceCenter.Status,
            CreatedAt = serviceCenter.CreatedAt,
            UpdatedAt = serviceCenter.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var serviceCenter = await _repository.GetByIdAsync(id, cancellationToken);
        if (serviceCenter is null) throw new NotFoundException("ServiceCenter", id);

        if (serviceCenter.OwnerUserId != userId)
            throw new ForbiddenException("You can only delete your own service centers.");

        await _repository.DeleteAsync(serviceCenter, cancellationToken);
    }

    public async Task<ServiceCenterBranchResponse> CreateBranchAsync(Guid serviceCenterId, CreateServiceCenterBranchRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var serviceCenter = await _repository.GetByIdAsync(serviceCenterId, cancellationToken);
        if (serviceCenter is null) throw new NotFoundException("ServiceCenter", serviceCenterId);

        if (serviceCenter.OwnerUserId != userId)
            throw new ForbiddenException("You can only add branches to your own service centers.");

        var branch = new ServiceCenterBranch
        {
            ServiceCenterId = serviceCenterId,
            Name = request.Name,
            Description = request.Description,
            LocationId = request.LocationId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            ContactPhone = request.ContactPhone,
            ContactEmail = request.ContactEmail,
            Status = ServiceCenterStatus.Active
        };

        await _branchRepository.AddAsync(branch, cancellationToken);

        return new ServiceCenterBranchResponse
        {
            Id = branch.Id,
            ServiceCenterId = branch.ServiceCenterId,
            ServiceCenterName = serviceCenter.Name,
            Name = branch.Name,
            Description = branch.Description,
            LocationId = branch.LocationId,
            LocationName = null,
            Latitude = branch.Latitude,
            Longitude = branch.Longitude,
            ContactPhone = branch.ContactPhone,
            ContactEmail = branch.ContactEmail,
            Status = branch.Status
        };
    }

    public async Task<ServiceCenterBranchResponse> UpdateBranchAsync(Guid serviceCenterId, Guid branchId, UpdateServiceCenterBranchRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var branch = await _branchRepository.GetByIdAsync(branchId, cancellationToken);
        if (branch is null) throw new NotFoundException("ServiceCenterBranch", branchId);

        if (branch.ServiceCenterId != serviceCenterId)
            throw new NotFoundException("ServiceCenterBranch", branchId);

        if (request.Name != null) branch.Name = request.Name;
        if (request.Description != null) branch.Description = request.Description;
        if (request.LocationId.HasValue) branch.LocationId = request.LocationId;
        if (request.Latitude.HasValue) branch.Latitude = request.Latitude;
        if (request.Longitude.HasValue) branch.Longitude = request.Longitude;
        if (request.ContactPhone != null) branch.ContactPhone = request.ContactPhone;
        if (request.ContactEmail != null) branch.ContactEmail = request.ContactEmail;

        await _branchRepository.UpdateAsync(branch, cancellationToken);

        return new ServiceCenterBranchResponse
        {
            Id = branch.Id,
            ServiceCenterId = branch.ServiceCenterId,
            ServiceCenterName = string.Empty,
            Name = branch.Name,
            Description = branch.Description,
            LocationId = branch.LocationId,
            LocationName = null,
            Latitude = branch.Latitude,
            Longitude = branch.Longitude,
            ContactPhone = branch.ContactPhone,
            ContactEmail = branch.ContactEmail,
            Status = branch.Status
        };
    }

    public async Task DeleteBranchAsync(Guid serviceCenterId, Guid branchId, Guid userId, CancellationToken cancellationToken = default)
    {
        var branch = await _branchRepository.GetByIdAsync(branchId, cancellationToken);
        if (branch is null) throw new NotFoundException("ServiceCenterBranch", branchId);

        if (branch.ServiceCenterId != serviceCenterId)
            throw new NotFoundException("ServiceCenterBranch", branchId);

        await _branchRepository.DeleteAsync(branch, cancellationToken);
    }

    public async Task<PagedResult<ServiceCenterBranchResponse>> GetBranchesAsync(Guid serviceCenterId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _branchRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(b => b.ServiceCenterId == serviceCenterId).OrderBy(b => b.Name);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new ServiceCenterBranchResponse
            {
                Id = b.Id,
                ServiceCenterId = b.ServiceCenterId,
                ServiceCenterName = string.Empty,
                Name = b.Name,
                Description = b.Description,
                LocationId = b.LocationId,
                LocationName = b.Location != null ? b.Location.Name : null,
                Latitude = b.Latitude,
                Longitude = b.Longitude,
                ContactPhone = b.ContactPhone,
                ContactEmail = b.ContactEmail,
                Status = b.Status
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ServiceCenterBranchResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ServiceCenterBranchResponse?> GetBranchByIdAsync(Guid serviceCenterId, Guid branchId, CancellationToken cancellationToken = default)
    {
        var branch = await _branchRepository.GetByIdAsync(branchId, cancellationToken);
        if (branch is null || branch.ServiceCenterId != serviceCenterId) return null;

        return new ServiceCenterBranchResponse
        {
            Id = branch.Id,
            ServiceCenterId = branch.ServiceCenterId,
            ServiceCenterName = string.Empty,
            Name = branch.Name,
            Description = branch.Description,
            LocationId = branch.LocationId,
            LocationName = branch.Location != null ? branch.Location.Name : null,
            Latitude = branch.Latitude,
            Longitude = branch.Longitude,
            ContactPhone = branch.ContactPhone,
            ContactEmail = branch.ContactEmail,
            Status = branch.Status
        };
    }

    public async Task<PagedResult<ServiceCenterServiceResponse>> GetBranchServicesAsync(Guid branchId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _branchRepository.QueryAsync(true, cancellationToken);
        var branch = await query.FirstOrDefaultAsync(b => b.Id == branchId, cancellationToken);
        if (branch is null) throw new NotFoundException("ServiceCenterBranch", branchId);

        var services = await _branchRepository.QueryAsync(true, cancellationToken);
        var branchServices = await services
            .Where(b => b.Id == branchId)
            .SelectMany(b => b.Services)
            .ToListAsync(cancellationToken);

        var totalCount = branchServices.Count;
        var items = branchServices
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new ServiceCenterServiceResponse
            {
                Id = s.Id,
                ServiceCenterBranchId = s.ServiceCenterBranchId,
                AutoServiceTypeId = s.AutoServiceTypeId,
                AutoServiceTypeName = s.AutoServiceType != null ? s.AutoServiceType.Name : string.Empty,
                PriceFrom = s.PriceFrom,
                PriceTo = s.PriceTo,
                EstimatedDurationMinutes = s.EstimatedDurationMinutes,
                IsAvailable = s.IsAvailable,
                Description = s.Description
            })
            .ToList();

        return PagedResult<ServiceCenterServiceResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<ServicePackageResponse>> GetBranchPackagesAsync(Guid branchId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _branchRepository.QueryAsync(true, cancellationToken);
        var branch = await query.FirstOrDefaultAsync(b => b.Id == branchId, cancellationToken);
        if (branch is null) throw new NotFoundException("ServiceCenterBranch", branchId);

        var packages = await _branchRepository.QueryAsync(true, cancellationToken);
        var branchPackages = await packages
            .Where(b => b.Id == branchId)
            .SelectMany(b => b.Packages)
            .ToListAsync(cancellationToken);

        var totalCount = branchPackages.Count;
        var items = branchPackages
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ServicePackageResponse
            {
                Id = p.Id,
                ServiceCenterBranchId = p.ServiceCenterBranchId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Currency = p.Currency,
                EstimatedDurationMinutes = p.EstimatedDurationMinutes,
                IsActive = p.IsActive
            })
            .ToList();

        return PagedResult<ServicePackageResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<ServiceWorkingHourResponse>> GetBranchWorkingHoursAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        var query = await _branchRepository.QueryAsync(true, cancellationToken);
        var branch = await query.FirstOrDefaultAsync(b => b.Id == branchId, cancellationToken);
        if (branch is null) throw new NotFoundException("ServiceCenterBranch", branchId);

        var workingHours = await _branchRepository.QueryAsync(true, cancellationToken);
        var branchHours = await workingHours
            .Where(b => b.Id == branchId)
            .SelectMany(b => b.WorkingHours)
            .ToListAsync(cancellationToken);

        var items = branchHours
            .Select(w => new ServiceWorkingHourResponse
            {
                Id = w.Id,
                ServiceCenterBranchId = w.ServiceCenterBranchId,
                DayOfWeek = w.DayOfWeek,
                OpeningTime = w.OpeningTime,
                ClosingTime = w.ClosingTime,
                IsClosed = w.IsClosed
            })
            .ToList();

        return PagedResult<ServiceWorkingHourResponse>.Create(items, items.Count, 1, items.Count);
    }

    public async Task<PagedResult<ServiceCenterListDto>> GetNearbyAsync(double latitude, double longitude, double radiusKm, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        var allCenters = await query
            .Where(sc => sc.Status == ServiceCenterStatus.Active)
            .ToListAsync(cancellationToken);

        var nearbyCenters = allCenters
            .Select(sc => new
            {
                ServiceCenter = sc,
                DistanceKm = CalculateDistance(latitude, longitude, sc.Branches.FirstOrDefault()?.Latitude, sc.Branches.FirstOrDefault()?.Longitude)
            })
            .Where(x => x.DistanceKm <= radiusKm)
            .OrderBy(x => x.DistanceKm)
            .ToList();

        var totalCount = nearbyCenters.Count;
        var items = nearbyCenters
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ServiceCenterListDto
            {
                Id = x.ServiceCenter.Id,
                Name = x.ServiceCenter.Name,
                Description = x.ServiceCenter.Description,
                ContactPhone = x.ServiceCenter.ContactPhone,
                ContactEmail = x.ServiceCenter.ContactEmail,
                WebsiteUrl = x.ServiceCenter.WebsiteUrl,
                VerificationStatus = x.ServiceCenter.VerificationStatus,
                Status = x.ServiceCenter.Status
            })
            .ToList();

        return PagedResult<ServiceCenterListDto>.Create(items, totalCount, page, pageSize);
    }

    private static double CalculateDistance(double lat1, double lon1, double? lat2, double? lon2)
    {
        if (!lat2.HasValue || !lon2.HasValue) return double.MaxValue;

        const double R = 6371;
        var dLat = ToRad(lat2.Value - lat1);
        var dLon = ToRad(lon2.Value - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2.Value)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRad(double degrees) => degrees * Math.PI / 180;
}
