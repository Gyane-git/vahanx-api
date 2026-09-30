using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Trust;
using VahanX.Domain.Entities;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of vehicle history service.
/// </summary>
public class VehicleHistoryService : IVehicleHistoryService
{
    private readonly IRepository<OwnershipHistory> _ownershipRepository;
    private readonly IRepository<MileageHistory> _mileageRepository;
    private readonly IRepository<ServiceHistory> _serviceRepository;
    private readonly IRepository<AccidentHistory> _accidentRepository;
    private readonly IRepository<InsuranceHistory> _insuranceRepository;
    private readonly IRepository<RegistrationHistory> _registrationRepository;
    private readonly IRepository<PriceHistory> _priceRepository;

    public VehicleHistoryService(
        IRepository<OwnershipHistory> ownershipRepository,
        IRepository<MileageHistory> mileageRepository,
        IRepository<ServiceHistory> serviceRepository,
        IRepository<AccidentHistory> accidentRepository,
        IRepository<InsuranceHistory> insuranceRepository,
        IRepository<RegistrationHistory> registrationRepository,
        IRepository<PriceHistory> priceRepository)
    {
        _ownershipRepository = ownershipRepository;
        _mileageRepository = mileageRepository;
        _serviceRepository = serviceRepository;
        _accidentRepository = accidentRepository;
        _insuranceRepository = insuranceRepository;
        _registrationRepository = registrationRepository;
        _priceRepository = priceRepository;
    }

    public async Task<VehicleHistoryResponse> GetVehicleHistoryAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var ownershipQuery = await _ownershipRepository.QueryAsync(true, cancellationToken);
        var mileageQuery = await _mileageRepository.QueryAsync(true, cancellationToken);
        var serviceQuery = await _serviceRepository.QueryAsync(true, cancellationToken);
        var accidentQuery = await _accidentRepository.QueryAsync(true, cancellationToken);
        var insuranceQuery = await _insuranceRepository.QueryAsync(true, cancellationToken);
        var registrationQuery = await _registrationRepository.QueryAsync(true, cancellationToken);

        var ownershipHistory = await ownershipQuery
            .Where(o => o.VehicleId == vehicleId)
            .OrderBy(o => o.FromDate)
            .Select(o => new OwnershipHistoryResponse
            {
                Id = o.Id,
                VehicleId = o.VehicleId,
                OwnerSequence = o.OwnerSequence,
                OwnershipType = o.OwnershipType,
                FromDate = o.FromDate,
                ToDate = o.ToDate,
                Source = o.Source,
                Notes = o.Notes
            })
            .ToListAsync(cancellationToken);

        var mileageHistory = await mileageQuery
            .Where(m => m.VehicleId == vehicleId)
            .OrderBy(m => m.RecordedAt)
            .Select(m => new MileageHistoryResponse
            {
                Id = m.Id,
                VehicleId = m.VehicleId,
                Mileage = m.Mileage,
                MileageUnit = m.MileageUnit,
                RecordedAt = m.RecordedAt,
                Source = m.Source,
                Notes = m.Notes
            })
            .ToListAsync(cancellationToken);

        var serviceHistory = await serviceQuery
            .Where(s => s.VehicleId == vehicleId)
            .OrderBy(s => s.ServiceDate)
            .Select(s => new ServiceHistoryResponse
            {
                Id = s.Id,
                VehicleId = s.VehicleId,
                ServiceDate = s.ServiceDate,
                Mileage = s.Mileage,
                ServiceType = s.ServiceType,
                ServiceProvider = s.ServiceProvider,
                Description = s.Description,
                Cost = s.Cost,
                Source = s.Source,
                Notes = s.Notes
            })
            .ToListAsync(cancellationToken);

        var accidentHistory = await accidentQuery
            .Where(a => a.VehicleId == vehicleId)
            .OrderBy(a => a.AccidentDate)
            .Select(a => new AccidentHistoryResponse
            {
                Id = a.Id,
                VehicleId = a.VehicleId,
                AccidentDate = a.AccidentDate,
                Severity = a.Severity,
                Description = a.Description,
                RepairStatus = a.RepairStatus,
                RepairCost = a.RepairCost,
                Source = a.Source,
                Notes = a.Notes
            })
            .ToListAsync(cancellationToken);

        var insuranceHistory = await insuranceQuery
            .Where(i => i.VehicleId == vehicleId)
            .OrderBy(i => i.StartDate)
            .Select(i => new InsuranceHistoryResponse
            {
                Id = i.Id,
                VehicleId = i.VehicleId,
                Provider = i.Provider,
                StartDate = i.StartDate,
                EndDate = i.EndDate,
                Status = i.Status,
                Source = i.Source
            })
            .ToListAsync(cancellationToken);

        var registrationHistory = await registrationQuery
            .Where(r => r.VehicleId == vehicleId)
            .OrderBy(r => r.RegistrationDate)
            .Select(r => new RegistrationHistoryResponse
            {
                Id = r.Id,
                VehicleId = r.VehicleId,
                RegistrationDate = r.RegistrationDate,
                RegistrationStatus = r.RegistrationStatus,
                RegistrationArea = r.RegistrationArea,
                Source = r.Source,
                Notes = r.Notes
            })
            .ToListAsync(cancellationToken);

        return new VehicleHistoryResponse
        {
            VehicleId = vehicleId,
            TotalPreviousOwners = ownershipHistory.Count,
            OwnershipHistory = ownershipHistory,
            MileageHistory = mileageHistory,
            ServiceHistory = serviceHistory,
            AccidentHistory = accidentHistory,
            InsuranceHistory = insuranceHistory,
            RegistrationHistory = registrationHistory
        };
    }

    public async Task<PagedResult<OwnershipHistoryResponse>> GetOwnershipHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _ownershipRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(o => o.VehicleId == vehicleId).OrderBy(o => o.FromDate);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OwnershipHistoryResponse
            {
                Id = o.Id,
                VehicleId = o.VehicleId,
                OwnerSequence = o.OwnerSequence,
                OwnershipType = o.OwnershipType,
                FromDate = o.FromDate,
                ToDate = o.ToDate,
                Source = o.Source,
                Notes = o.Notes
            })
            .ToListAsync(cancellationToken);

        return PagedResult<OwnershipHistoryResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<MileageHistoryResponse>> GetMileageHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _mileageRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(m => m.VehicleId == vehicleId).OrderBy(m => m.RecordedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MileageHistoryResponse
            {
                Id = m.Id,
                VehicleId = m.VehicleId,
                Mileage = m.Mileage,
                MileageUnit = m.MileageUnit,
                RecordedAt = m.RecordedAt,
                Source = m.Source,
                Notes = m.Notes
            })
            .ToListAsync(cancellationToken);

        return PagedResult<MileageHistoryResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<ServiceHistoryResponse>> GetServiceHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _serviceRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(s => s.VehicleId == vehicleId).OrderBy(s => s.ServiceDate);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new ServiceHistoryResponse
            {
                Id = s.Id,
                VehicleId = s.VehicleId,
                ServiceDate = s.ServiceDate,
                Mileage = s.Mileage,
                ServiceType = s.ServiceType,
                ServiceProvider = s.ServiceProvider,
                Description = s.Description,
                Cost = s.Cost,
                Source = s.Source,
                Notes = s.Notes
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ServiceHistoryResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<AccidentHistoryResponse>> GetAccidentHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _accidentRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(a => a.VehicleId == vehicleId).OrderBy(a => a.AccidentDate);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AccidentHistoryResponse
            {
                Id = a.Id,
                VehicleId = a.VehicleId,
                AccidentDate = a.AccidentDate,
                Severity = a.Severity,
                Description = a.Description,
                RepairStatus = a.RepairStatus,
                RepairCost = a.RepairCost,
                Source = a.Source,
                Notes = a.Notes
            })
            .ToListAsync(cancellationToken);

        return PagedResult<AccidentHistoryResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<InsuranceHistoryResponse>> GetInsuranceHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _insuranceRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(i => i.VehicleId == vehicleId).OrderBy(i => i.StartDate);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new InsuranceHistoryResponse
            {
                Id = i.Id,
                VehicleId = i.VehicleId,
                Provider = i.Provider,
                StartDate = i.StartDate,
                EndDate = i.EndDate,
                Status = i.Status,
                Source = i.Source
            })
            .ToListAsync(cancellationToken);

        return PagedResult<InsuranceHistoryResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<RegistrationHistoryResponse>> GetRegistrationHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _registrationRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(r => r.VehicleId == vehicleId).OrderBy(r => r.RegistrationDate);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RegistrationHistoryResponse
            {
                Id = r.Id,
                VehicleId = r.VehicleId,
                RegistrationDate = r.RegistrationDate,
                RegistrationStatus = r.RegistrationStatus,
                RegistrationArea = r.RegistrationArea,
                Source = r.Source,
                Notes = r.Notes
            })
            .ToListAsync(cancellationToken);

        return PagedResult<RegistrationHistoryResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<PriceHistoryResponse>> GetPriceHistoryAsync(Guid listingId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _priceRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(p => p.ListingId == listingId).OrderBy(p => p.ChangedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PriceHistoryResponse
            {
                Id = p.Id,
                ListingId = p.ListingId,
                OldPrice = p.OldPrice,
                NewPrice = p.NewPrice,
                Currency = p.Currency,
                ChangedAt = p.ChangedAt,
                Reason = p.Reason
            })
            .ToListAsync(cancellationToken);

        return PagedResult<PriceHistoryResponse>.Create(items, totalCount, page, pageSize);
    }
}
