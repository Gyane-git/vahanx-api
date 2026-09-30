using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of service booking service.
/// </summary>
public class ServiceBookingService : IServiceBookingService
{
    private readonly IRepository<ServiceBooking> _repository;
    private readonly IRepository<ServiceCenterBranch> _branchRepository;

    public ServiceBookingService(
        IRepository<ServiceBooking> repository,
        IRepository<ServiceCenterBranch> branchRepository)
    {
        _repository = repository;
        _branchRepository = branchRepository;
    }

    public async Task<PagedResult<ServiceBookingResponse>> GetAllAsync(int page, int pageSize, Guid? userId = null, Guid? branchId = null, ServiceBookingStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (userId.HasValue)
            query = query.Where(b => b.UserId == userId.Value);
        if (branchId.HasValue)
            query = query.Where(b => b.ServiceCenterBranchId == branchId.Value);
        if (status.HasValue)
            query = query.Where(b => b.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new ServiceBookingResponse
            {
                Id = b.Id,
                UserId = b.UserId,
                ServiceCenterBranchId = b.ServiceCenterBranchId,
                ServiceCenterBranchName = string.Empty,
                VehicleId = b.VehicleId,
                VehicleName = null,
                AutoServiceTypeId = b.AutoServiceTypeId,
                AutoServiceTypeName = null,
                ServicePackageId = b.ServicePackageId,
                ServicePackageName = null,
                BookingReference = b.BookingReference,
                RequestedDate = b.RequestedDate,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status,
                CustomerNotes = b.CustomerNotes,
                CenterNotes = b.CenterNotes,
                EstimatedPrice = b.EstimatedPrice,
                FinalPrice = b.FinalPrice,
                Currency = b.Currency,
                ConfirmedAt = b.ConfirmedAt,
                CompletedAt = b.CompletedAt,
                CancelledAt = b.CancelledAt,
                CancellationReason = b.CancellationReason,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ServiceBookingResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ServiceBookingResponse?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, cancellationToken);
        if (booking is null) return null;

        if (booking.UserId != userId)
            throw new ForbiddenException("You do not have access to this booking.");

        return new ServiceBookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceCenterBranchId = booking.ServiceCenterBranchId,
            ServiceCenterBranchName = string.Empty,
            VehicleId = booking.VehicleId,
            VehicleName = null,
            AutoServiceTypeId = booking.AutoServiceTypeId,
            AutoServiceTypeName = null,
            ServicePackageId = booking.ServicePackageId,
            ServicePackageName = null,
            BookingReference = booking.BookingReference,
            RequestedDate = booking.RequestedDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CustomerNotes = booking.CustomerNotes,
            CenterNotes = booking.CenterNotes,
            EstimatedPrice = booking.EstimatedPrice,
            FinalPrice = booking.FinalPrice,
            Currency = booking.Currency,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public async Task<ServiceBookingResponse> CreateAsync(CreateServiceBookingRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var branch = await _branchRepository.GetByIdAsync(request.ServiceCenterBranchId, cancellationToken);
        if (branch is null) throw new NotFoundException("ServiceCenterBranch", request.ServiceCenterBranchId);

        if (branch.Status != ServiceCenterStatus.Active)
            throw new ConflictException("Cannot book at an inactive branch.");

        var booking = new ServiceBooking
        {
            UserId = userId,
            ServiceCenterBranchId = request.ServiceCenterBranchId,
            VehicleId = request.VehicleId,
            AutoServiceTypeId = request.AutoServiceTypeId,
            ServicePackageId = request.ServicePackageId,
            BookingReference = GenerateBookingReference(),
            RequestedDate = request.RequestedDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = ServiceBookingStatus.Requested,
            CustomerNotes = request.CustomerNotes
        };

        await _repository.AddAsync(booking, cancellationToken);

        return new ServiceBookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceCenterBranchId = booking.ServiceCenterBranchId,
            ServiceCenterBranchName = branch.Name,
            VehicleId = booking.VehicleId,
            VehicleName = null,
            AutoServiceTypeId = booking.AutoServiceTypeId,
            AutoServiceTypeName = null,
            ServicePackageId = booking.ServicePackageId,
            ServicePackageName = null,
            BookingReference = booking.BookingReference,
            RequestedDate = booking.RequestedDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CustomerNotes = booking.CustomerNotes,
            CenterNotes = booking.CenterNotes,
            EstimatedPrice = booking.EstimatedPrice,
            FinalPrice = booking.FinalPrice,
            Currency = booking.Currency,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public async Task<ServiceBookingResponse> UpdateAsync(Guid id, UpdateServiceBookingRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, cancellationToken);
        if (booking is null) throw new NotFoundException("ServiceBooking", id);

        if (booking.UserId != userId)
            throw new ForbiddenException("You can only update your own bookings.");

        if (booking.Status == ServiceBookingStatus.Completed || booking.Status == ServiceBookingStatus.Cancelled)
            throw new ConflictException($"Cannot update a booking with status {booking.Status}.");

        if (request.RequestedDate.HasValue) booking.RequestedDate = request.RequestedDate.Value;
        if (request.StartTime.HasValue) booking.StartTime = request.StartTime;
        if (request.EndTime.HasValue) booking.EndTime = request.EndTime;
        if (request.CustomerNotes != null) booking.CustomerNotes = request.CustomerNotes;

        await _repository.UpdateAsync(booking, cancellationToken);

        return new ServiceBookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceCenterBranchId = booking.ServiceCenterBranchId,
            ServiceCenterBranchName = string.Empty,
            VehicleId = booking.VehicleId,
            VehicleName = null,
            AutoServiceTypeId = booking.AutoServiceTypeId,
            AutoServiceTypeName = null,
            ServicePackageId = booking.ServicePackageId,
            ServicePackageName = null,
            BookingReference = booking.BookingReference,
            RequestedDate = booking.RequestedDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CustomerNotes = booking.CustomerNotes,
            CenterNotes = booking.CenterNotes,
            EstimatedPrice = booking.EstimatedPrice,
            FinalPrice = booking.FinalPrice,
            Currency = booking.Currency,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, cancellationToken);
        if (booking is null) throw new NotFoundException("ServiceBooking", id);

        if (booking.UserId != userId)
            throw new ForbiddenException("You can only delete your own bookings.");

        await _repository.DeleteAsync(booking, cancellationToken);
    }

    public async Task<ServiceBookingResponse> ConfirmAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, cancellationToken);
        if (booking is null) throw new NotFoundException("ServiceBooking", id);

        if (booking.Status != ServiceBookingStatus.Requested && booking.Status != ServiceBookingStatus.Pending)
            throw new ConflictException($"Cannot confirm a booking with status {booking.Status}.");

        booking.Status = ServiceBookingStatus.Confirmed;
        booking.ConfirmedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(booking, cancellationToken);

        return new ServiceBookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceCenterBranchId = booking.ServiceCenterBranchId,
            ServiceCenterBranchName = string.Empty,
            VehicleId = booking.VehicleId,
            VehicleName = null,
            AutoServiceTypeId = booking.AutoServiceTypeId,
            AutoServiceTypeName = null,
            ServicePackageId = booking.ServicePackageId,
            ServicePackageName = null,
            BookingReference = booking.BookingReference,
            RequestedDate = booking.RequestedDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CustomerNotes = booking.CustomerNotes,
            CenterNotes = booking.CenterNotes,
            EstimatedPrice = booking.EstimatedPrice,
            FinalPrice = booking.FinalPrice,
            Currency = booking.Currency,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public async Task<ServiceBookingResponse> RescheduleAsync(Guid id, RescheduleServiceBookingRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, cancellationToken);
        if (booking is null) throw new NotFoundException("ServiceBooking", id);

        if (booking.Status != ServiceBookingStatus.Confirmed && booking.Status != ServiceBookingStatus.Requested)
            throw new ConflictException($"Cannot reschedule a booking with status {booking.Status}.");

        booking.StartTime = request.NewStartTime;
        booking.EndTime = request.NewEndTime;
        booking.Status = ServiceBookingStatus.Rescheduled;
        await _repository.UpdateAsync(booking, cancellationToken);

        return new ServiceBookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceCenterBranchId = booking.ServiceCenterBranchId,
            ServiceCenterBranchName = string.Empty,
            VehicleId = booking.VehicleId,
            VehicleName = null,
            AutoServiceTypeId = booking.AutoServiceTypeId,
            AutoServiceTypeName = null,
            ServicePackageId = booking.ServicePackageId,
            ServicePackageName = null,
            BookingReference = booking.BookingReference,
            RequestedDate = booking.RequestedDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CustomerNotes = booking.CustomerNotes,
            CenterNotes = booking.CenterNotes,
            EstimatedPrice = booking.EstimatedPrice,
            FinalPrice = booking.FinalPrice,
            Currency = booking.Currency,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public async Task<ServiceBookingResponse> StartAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, cancellationToken);
        if (booking is null) throw new NotFoundException("ServiceBooking", id);

        if (booking.Status != ServiceBookingStatus.Confirmed && booking.Status != ServiceBookingStatus.Rescheduled)
            throw new ConflictException($"Cannot start a booking with status {booking.Status}.");

        booking.Status = ServiceBookingStatus.InProgress;
        await _repository.UpdateAsync(booking, cancellationToken);

        return new ServiceBookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceCenterBranchId = booking.ServiceCenterBranchId,
            ServiceCenterBranchName = string.Empty,
            VehicleId = booking.VehicleId,
            VehicleName = null,
            AutoServiceTypeId = booking.AutoServiceTypeId,
            AutoServiceTypeName = null,
            ServicePackageId = booking.ServicePackageId,
            ServicePackageName = null,
            BookingReference = booking.BookingReference,
            RequestedDate = booking.RequestedDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CustomerNotes = booking.CustomerNotes,
            CenterNotes = booking.CenterNotes,
            EstimatedPrice = booking.EstimatedPrice,
            FinalPrice = booking.FinalPrice,
            Currency = booking.Currency,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public async Task<ServiceBookingResponse> CompleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, cancellationToken);
        if (booking is null) throw new NotFoundException("ServiceBooking", id);

        if (booking.Status != ServiceBookingStatus.InProgress)
            throw new ConflictException($"Cannot complete a booking with status {booking.Status}.");

        booking.Status = ServiceBookingStatus.Completed;
        booking.CompletedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(booking, cancellationToken);

        return new ServiceBookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceCenterBranchId = booking.ServiceCenterBranchId,
            ServiceCenterBranchName = string.Empty,
            VehicleId = booking.VehicleId,
            VehicleName = null,
            AutoServiceTypeId = booking.AutoServiceTypeId,
            AutoServiceTypeName = null,
            ServicePackageId = booking.ServicePackageId,
            ServicePackageName = null,
            BookingReference = booking.BookingReference,
            RequestedDate = booking.RequestedDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CustomerNotes = booking.CustomerNotes,
            CenterNotes = booking.CenterNotes,
            EstimatedPrice = booking.EstimatedPrice,
            FinalPrice = booking.FinalPrice,
            Currency = booking.Currency,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public async Task<ServiceBookingResponse> CancelAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, cancellationToken);
        if (booking is null) throw new NotFoundException("ServiceBooking", id);

        if (booking.Status == ServiceBookingStatus.Completed || booking.Status == ServiceBookingStatus.Cancelled)
            throw new ConflictException($"Cannot cancel a booking with status {booking.Status}.");

        booking.Status = ServiceBookingStatus.Cancelled;
        booking.CancelledAt = DateTime.UtcNow;
        await _repository.UpdateAsync(booking, cancellationToken);

        return new ServiceBookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceCenterBranchId = booking.ServiceCenterBranchId,
            ServiceCenterBranchName = string.Empty,
            VehicleId = booking.VehicleId,
            VehicleName = null,
            AutoServiceTypeId = booking.AutoServiceTypeId,
            AutoServiceTypeName = null,
            ServicePackageId = booking.ServicePackageId,
            ServicePackageName = null,
            BookingReference = booking.BookingReference,
            RequestedDate = booking.RequestedDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CustomerNotes = booking.CustomerNotes,
            CenterNotes = booking.CenterNotes,
            EstimatedPrice = booking.EstimatedPrice,
            FinalPrice = booking.FinalPrice,
            Currency = booking.Currency,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public async Task<ServiceBookingResponse> RejectAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, cancellationToken);
        if (booking is null) throw new NotFoundException("ServiceBooking", id);

        if (booking.Status != ServiceBookingStatus.Requested && booking.Status != ServiceBookingStatus.Pending)
            throw new ConflictException($"Cannot reject a booking with status {booking.Status}.");

        booking.Status = ServiceBookingStatus.Rejected;
        await _repository.UpdateAsync(booking, cancellationToken);

        return new ServiceBookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceCenterBranchId = booking.ServiceCenterBranchId,
            ServiceCenterBranchName = string.Empty,
            VehicleId = booking.VehicleId,
            VehicleName = null,
            AutoServiceTypeId = booking.AutoServiceTypeId,
            AutoServiceTypeName = null,
            ServicePackageId = booking.ServicePackageId,
            ServicePackageName = null,
            BookingReference = booking.BookingReference,
            RequestedDate = booking.RequestedDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CustomerNotes = booking.CustomerNotes,
            CenterNotes = booking.CenterNotes,
            EstimatedPrice = booking.EstimatedPrice,
            FinalPrice = booking.FinalPrice,
            Currency = booking.Currency,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public async Task<ServiceBookingResponse> NoShowAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, cancellationToken);
        if (booking is null) throw new NotFoundException("ServiceBooking", id);

        if (booking.Status != ServiceBookingStatus.Confirmed && booking.Status != ServiceBookingStatus.Rescheduled)
            throw new ConflictException($"Cannot mark as no-show a booking with status {booking.Status}.");

        booking.Status = ServiceBookingStatus.NoShow;
        await _repository.UpdateAsync(booking, cancellationToken);

        return new ServiceBookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceCenterBranchId = booking.ServiceCenterBranchId,
            ServiceCenterBranchName = string.Empty,
            VehicleId = booking.VehicleId,
            VehicleName = null,
            AutoServiceTypeId = booking.AutoServiceTypeId,
            AutoServiceTypeName = null,
            ServicePackageId = booking.ServicePackageId,
            ServicePackageName = null,
            BookingReference = booking.BookingReference,
            RequestedDate = booking.RequestedDate,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CustomerNotes = booking.CustomerNotes,
            CenterNotes = booking.CenterNotes,
            EstimatedPrice = booking.EstimatedPrice,
            FinalPrice = booking.FinalPrice,
            Currency = booking.Currency,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public async Task<PagedResult<ServiceBookingResponse>> GetMyBookingsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(b => b.UserId == userId).OrderByDescending(b => b.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new ServiceBookingResponse
            {
                Id = b.Id,
                UserId = b.UserId,
                ServiceCenterBranchId = b.ServiceCenterBranchId,
                ServiceCenterBranchName = string.Empty,
                VehicleId = b.VehicleId,
                VehicleName = null,
                AutoServiceTypeId = b.AutoServiceTypeId,
                AutoServiceTypeName = null,
                ServicePackageId = b.ServicePackageId,
                ServicePackageName = null,
                BookingReference = b.BookingReference,
                RequestedDate = b.RequestedDate,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status,
                CustomerNotes = b.CustomerNotes,
                CenterNotes = b.CenterNotes,
                EstimatedPrice = b.EstimatedPrice,
                FinalPrice = b.FinalPrice,
                Currency = b.Currency,
                ConfirmedAt = b.ConfirmedAt,
                CompletedAt = b.CompletedAt,
                CancelledAt = b.CancelledAt,
                CancellationReason = b.CancellationReason,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ServiceBookingResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<ServiceBookingResponse>> GetBranchBookingsAsync(Guid branchId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(b => b.ServiceCenterBranchId == branchId).OrderByDescending(b => b.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new ServiceBookingResponse
            {
                Id = b.Id,
                UserId = b.UserId,
                ServiceCenterBranchId = b.ServiceCenterBranchId,
                ServiceCenterBranchName = string.Empty,
                VehicleId = b.VehicleId,
                VehicleName = null,
                AutoServiceTypeId = b.AutoServiceTypeId,
                AutoServiceTypeName = null,
                ServicePackageId = b.ServicePackageId,
                ServicePackageName = null,
                BookingReference = b.BookingReference,
                RequestedDate = b.RequestedDate,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status,
                CustomerNotes = b.CustomerNotes,
                CenterNotes = b.CenterNotes,
                EstimatedPrice = b.EstimatedPrice,
                FinalPrice = b.FinalPrice,
                Currency = b.Currency,
                ConfirmedAt = b.ConfirmedAt,
                CompletedAt = b.CompletedAt,
                CancelledAt = b.CancelledAt,
                CancellationReason = b.CancellationReason,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ServiceBookingResponse>.Create(items, totalCount, page, pageSize);
    }

    private static string GenerateBookingReference()
    {
        return $"SB-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
    }
}
