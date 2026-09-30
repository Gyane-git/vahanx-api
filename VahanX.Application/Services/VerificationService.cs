using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Trust;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of verification service.
/// </summary>
public class VerificationService : IVerificationService
{
    private readonly IRepository<VehicleVerification> _repository;
    private readonly IRepository<Vehicle> _vehicleRepository;
    private readonly IRepository<VehicleListing> _listingRepository;
    private readonly IRepository<VehicleVerificationStatusHistory> _historyRepository;

    public VerificationService(
        IRepository<VehicleVerification> repository,
        IRepository<Vehicle> vehicleRepository,
        IRepository<VehicleListing> listingRepository,
        IRepository<VehicleVerificationStatusHistory> historyRepository)
    {
        _repository = repository;
        _vehicleRepository = vehicleRepository;
        _listingRepository = listingRepository;
        _historyRepository = historyRepository;
    }

    public async Task<PagedResult<VerificationResponse>> GetAllAsync(int page, int pageSize, Guid? vehicleId = null, Guid? listingId = null, VehicleVerificationStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (vehicleId.HasValue)
            query = query.Where(v => v.VehicleId == vehicleId.Value);

        if (listingId.HasValue)
            query = query.Where(v => v.ListingId == listingId.Value);

        if (status.HasValue)
            query = query.Where(v => v.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(v => v.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new VerificationResponse
            {
                Id = v.Id,
                VehicleId = v.VehicleId,
                ListingId = v.ListingId,
                Status = v.Status,
                VerificationType = v.VerificationType,
                VerifiedByUserId = v.VerifiedByUserId,
                VerifiedAt = v.VerifiedAt,
                ExpiryAt = v.ExpiryAt,
                VerificationReference = v.VerificationReference,
                Notes = v.Notes,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<VerificationResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<VerificationResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var verification = await _repository.GetByIdAsync(id, cancellationToken);

        if (verification is null)
            return null;

        return new VerificationResponse
        {
            Id = verification.Id,
            VehicleId = verification.VehicleId,
            ListingId = verification.ListingId,
            Status = verification.Status,
            VerificationType = verification.VerificationType,
            VerifiedByUserId = verification.VerifiedByUserId,
            VerifiedAt = verification.VerifiedAt,
            ExpiryAt = verification.ExpiryAt,
            VerificationReference = verification.VerificationReference,
            Notes = verification.Notes,
            CreatedAt = verification.CreatedAt,
            UpdatedAt = verification.UpdatedAt
        };
    }

    public async Task<PublicVerificationResponse?> GetPublicByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var verifications = await _repository.QueryAsync(true, cancellationToken);
        var verification = await verifications
            .Where(v => v.VehicleId == vehicleId && v.Status == VehicleVerificationStatus.Verified)
            .OrderByDescending(v => v.VerifiedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (verification is null)
            return null;

        return new PublicVerificationResponse
        {
            Id = verification.Id,
            Status = verification.Status,
            VerificationType = verification.VerificationType,
            VerifiedAt = verification.VerifiedAt,
            ExpiryAt = verification.ExpiryAt,
            VerificationReference = verification.VerificationReference
        };
    }

    public async Task<VerificationResponse> CreateAsync(CreateVerificationRequest request, CancellationToken cancellationToken = default)
    {
        var vehicleExists = await _vehicleRepository.AnyAsync(v => v.Id == request.VehicleId, cancellationToken);
        if (!vehicleExists) throw new NotFoundException("Vehicle", request.VehicleId);

        if (request.ListingId.HasValue)
        {
            var listingExists = await _listingRepository.AnyAsync(l => l.Id == request.ListingId.Value, cancellationToken);
            if (!listingExists) throw new NotFoundException("Listing", request.ListingId.Value);
        }

        var verification = new VehicleVerification
        {
            VehicleId = request.VehicleId,
            ListingId = request.ListingId,
            Status = VehicleVerificationStatus.Pending,
            VerificationType = request.VerificationType,
            Notes = request.Notes
        };

        await _repository.AddAsync(verification, cancellationToken);

        await AddStatusHistoryAsync(verification.Id, VehicleVerificationStatus.NotVerified, VehicleVerificationStatus.Pending, null, "Verification created", cancellationToken);

        return new VerificationResponse
        {
            Id = verification.Id,
            VehicleId = verification.VehicleId,
            ListingId = verification.ListingId,
            Status = verification.Status,
            VerificationType = verification.VerificationType,
            VerifiedByUserId = verification.VerifiedByUserId,
            VerifiedAt = verification.VerifiedAt,
            ExpiryAt = verification.ExpiryAt,
            VerificationReference = verification.VerificationReference,
            Notes = verification.Notes,
            CreatedAt = verification.CreatedAt,
            UpdatedAt = verification.UpdatedAt
        };
    }

    public async Task<VerificationResponse> UpdateAsync(Guid id, UpdateVerificationRequest request, CancellationToken cancellationToken = default)
    {
        var verification = await _repository.GetByIdAsync(id, cancellationToken);
        if (verification is null) throw new NotFoundException("Verification", id);

        if (verification.Status == VehicleVerificationStatus.Verified || verification.Status == VehicleVerificationStatus.Rejected)
            throw new ConflictException($"Cannot update a verification with status {verification.Status}.");

        if (request.VerificationType.HasValue)
            verification.VerificationType = request.VerificationType.Value;
        if (request.Notes != null)
            verification.Notes = request.Notes;
        if (request.ExpiryAt.HasValue)
            verification.ExpiryAt = request.ExpiryAt;

        await _repository.UpdateAsync(verification, cancellationToken);

        return new VerificationResponse
        {
            Id = verification.Id,
            VehicleId = verification.VehicleId,
            ListingId = verification.ListingId,
            Status = verification.Status,
            VerificationType = verification.VerificationType,
            VerifiedByUserId = verification.VerifiedByUserId,
            VerifiedAt = verification.VerifiedAt,
            ExpiryAt = verification.ExpiryAt,
            VerificationReference = verification.VerificationReference,
            Notes = verification.Notes,
            CreatedAt = verification.CreatedAt,
            UpdatedAt = verification.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var verification = await _repository.GetByIdAsync(id, cancellationToken);
        if (verification is null) throw new NotFoundException("Verification", id);

        await _repository.DeleteAsync(verification, cancellationToken);
    }

    public async Task<VerificationResponse> SubmitAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var verification = await _repository.GetByIdAsync(id, cancellationToken);
        if (verification is null) throw new NotFoundException("Verification", id);

        if (verification.Status != VehicleVerificationStatus.Pending && verification.Status != VehicleVerificationStatus.NotVerified)
            throw new ConflictException($"Cannot submit a verification with status {verification.Status}.");

        var oldStatus = verification.Status;
        verification.Status = VehicleVerificationStatus.InReview;
        await _repository.UpdateAsync(verification, cancellationToken);

        await AddStatusHistoryAsync(verification.Id, oldStatus, VehicleVerificationStatus.InReview, null, "Verification submitted for review", cancellationToken);

        return new VerificationResponse
        {
            Id = verification.Id,
            VehicleId = verification.VehicleId,
            ListingId = verification.ListingId,
            Status = verification.Status,
            VerificationType = verification.VerificationType,
            VerifiedByUserId = verification.VerifiedByUserId,
            VerifiedAt = verification.VerifiedAt,
            ExpiryAt = verification.ExpiryAt,
            VerificationReference = verification.VerificationReference,
            Notes = verification.Notes,
            CreatedAt = verification.CreatedAt,
            UpdatedAt = verification.UpdatedAt
        };
    }

    public async Task<VerificationResponse> ApproveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var verification = await _repository.GetByIdAsync(id, cancellationToken);
        if (verification is null) throw new NotFoundException("Verification", id);

        if (verification.Status != VehicleVerificationStatus.InReview && verification.Status != VehicleVerificationStatus.Pending)
            throw new ConflictException($"Cannot approve a verification with status {verification.Status}.");

        var oldStatus = verification.Status;
        verification.Status = VehicleVerificationStatus.Verified;
        verification.VerifiedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(verification, cancellationToken);

        await AddStatusHistoryAsync(verification.Id, oldStatus, VehicleVerificationStatus.Verified, null, "Verification approved", cancellationToken);

        return new VerificationResponse
        {
            Id = verification.Id,
            VehicleId = verification.VehicleId,
            ListingId = verification.ListingId,
            Status = verification.Status,
            VerificationType = verification.VerificationType,
            VerifiedByUserId = verification.VerifiedByUserId,
            VerifiedAt = verification.VerifiedAt,
            ExpiryAt = verification.ExpiryAt,
            VerificationReference = verification.VerificationReference,
            Notes = verification.Notes,
            CreatedAt = verification.CreatedAt,
            UpdatedAt = verification.UpdatedAt
        };
    }

    public async Task<VerificationResponse> RejectAsync(Guid id, string reason, CancellationToken cancellationToken = default)
    {
        var verification = await _repository.GetByIdAsync(id, cancellationToken);
        if (verification is null) throw new NotFoundException("Verification", id);

        if (verification.Status != VehicleVerificationStatus.InReview && verification.Status != VehicleVerificationStatus.Pending)
            throw new ConflictException($"Cannot reject a verification with status {verification.Status}.");

        var oldStatus = verification.Status;
        verification.Status = VehicleVerificationStatus.Rejected;
        await _repository.UpdateAsync(verification, cancellationToken);

        await AddStatusHistoryAsync(verification.Id, oldStatus, VehicleVerificationStatus.Rejected, null, reason, cancellationToken);

        return new VerificationResponse
        {
            Id = verification.Id,
            VehicleId = verification.VehicleId,
            ListingId = verification.ListingId,
            Status = verification.Status,
            VerificationType = verification.VerificationType,
            VerifiedByUserId = verification.VerifiedByUserId,
            VerifiedAt = verification.VerifiedAt,
            ExpiryAt = verification.ExpiryAt,
            VerificationReference = verification.VerificationReference,
            Notes = verification.Notes,
            CreatedAt = verification.CreatedAt,
            UpdatedAt = verification.UpdatedAt
        };
    }

    public async Task<VerificationResponse> RevokeAsync(Guid id, string reason, CancellationToken cancellationToken = default)
    {
        var verification = await _repository.GetByIdAsync(id, cancellationToken);
        if (verification is null) throw new NotFoundException("Verification", id);

        if (verification.Status != VehicleVerificationStatus.Verified)
            throw new ConflictException($"Cannot revoke a verification with status {verification.Status}.");

        var oldStatus = verification.Status;
        verification.Status = VehicleVerificationStatus.Revoked;
        await _repository.UpdateAsync(verification, cancellationToken);

        await AddStatusHistoryAsync(verification.Id, oldStatus, VehicleVerificationStatus.Revoked, null, reason, cancellationToken);

        return new VerificationResponse
        {
            Id = verification.Id,
            VehicleId = verification.VehicleId,
            ListingId = verification.ListingId,
            Status = verification.Status,
            VerificationType = verification.VerificationType,
            VerifiedByUserId = verification.VerifiedByUserId,
            VerifiedAt = verification.VerifiedAt,
            ExpiryAt = verification.ExpiryAt,
            VerificationReference = verification.VerificationReference,
            Notes = verification.Notes,
            CreatedAt = verification.CreatedAt,
            UpdatedAt = verification.UpdatedAt
        };
    }

    private async Task AddStatusHistoryAsync(Guid verificationId, VehicleVerificationStatus oldStatus, VehicleVerificationStatus newStatus, Guid? changedBy, string? reason, CancellationToken cancellationToken)
    {
        var history = new VehicleVerificationStatusHistory
        {
            VerificationId = verificationId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            ChangedBy = changedBy,
            ChangedAt = DateTime.UtcNow,
            Reason = reason
        };

        await _historyRepository.AddAsync(history, cancellationToken);
    }
}
