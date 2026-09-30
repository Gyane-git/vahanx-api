using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Engagement;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of test drive service.
/// </summary>
public class TestDriveService : ITestDriveService
{
    private readonly IRepository<TestDrive> _repository;
    private readonly IRepository<VehicleListing> _listingRepository;

    public TestDriveService(
        IRepository<TestDrive> repository,
        IRepository<VehicleListing> listingRepository)
    {
        _repository = repository;
        _listingRepository = listingRepository;
    }

    public async Task<PagedResult<TestDriveResponse>> GetAllAsync(int page, int pageSize, Guid? userId = null, Guid? listingId = null, Guid? dealerId = null, TestDriveStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (userId.HasValue)
            query = query.Where(t => t.UserId == userId.Value);
        if (listingId.HasValue)
            query = query.Where(t => t.ListingId == listingId.Value);
        if (dealerId.HasValue)
            query = query.Where(t => t.DealerId == dealerId.Value);
        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TestDriveResponse
            {
                Id = t.Id,
                UserId = t.UserId,
                ListingId = t.ListingId,
                ListingTitle = t.Listing != null ? t.Listing.Title : string.Empty,
                SellerId = t.SellerId,
                SellerName = t.Seller != null ? t.Seller.DisplayName : null,
                DealerId = t.DealerId,
                DealerName = t.Dealer != null ? t.Dealer.BusinessName : null,
                RequestedDate = t.RequestedDate,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                LocationId = t.LocationId,
                LocationName = t.Location != null ? t.Location.Name : null,
                Notes = t.Notes,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                CancelledAt = t.CancelledAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<TestDriveResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<TestDriveResponse?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var testDrive = await _repository.GetByIdAsync(id, cancellationToken);
        if (testDrive is null) return null;

        if (testDrive.UserId != userId && testDrive.SellerId == null && testDrive.DealerId == null)
            throw new ForbiddenException("You do not have access to this test drive.");

        return new TestDriveResponse
        {
            Id = testDrive.Id,
            UserId = testDrive.UserId,
            ListingId = testDrive.ListingId,
            ListingTitle = testDrive.Listing != null ? testDrive.Listing.Title : string.Empty,
            SellerId = testDrive.SellerId,
            SellerName = testDrive.Seller != null ? testDrive.Seller.DisplayName : null,
            DealerId = testDrive.DealerId,
            DealerName = testDrive.Dealer != null ? testDrive.Dealer.BusinessName : null,
            RequestedDate = testDrive.RequestedDate,
            StartTime = testDrive.StartTime,
            EndTime = testDrive.EndTime,
            LocationId = testDrive.LocationId,
            LocationName = testDrive.Location != null ? testDrive.Location.Name : null,
            Notes = testDrive.Notes,
            Status = testDrive.Status,
            CreatedAt = testDrive.CreatedAt,
            UpdatedAt = testDrive.UpdatedAt,
            CancelledAt = testDrive.CancelledAt
        };
    }

    public async Task<TestDriveResponse> CreateAsync(CreateTestDriveRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", request.ListingId);

        if (listing.Status != ListingStatus.Published)
            throw new ConflictException("Can only request test drives for published listings.");

        var testDrive = new TestDrive
        {
            UserId = userId,
            ListingId = request.ListingId,
            SellerId = listing.SellerId,
            DealerId = listing.DealerId,
            RequestedDate = request.RequestedDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            LocationId = request.LocationId,
            Notes = request.Notes,
            Status = TestDriveStatus.Requested
        };

        await _repository.AddAsync(testDrive, cancellationToken);

        return new TestDriveResponse
        {
            Id = testDrive.Id,
            UserId = testDrive.UserId,
            ListingId = testDrive.ListingId,
            ListingTitle = listing.Title,
            SellerId = testDrive.SellerId,
            SellerName = null,
            DealerId = testDrive.DealerId,
            DealerName = null,
            RequestedDate = testDrive.RequestedDate,
            StartTime = testDrive.StartTime,
            EndTime = testDrive.EndTime,
            LocationId = testDrive.LocationId,
            LocationName = null,
            Notes = testDrive.Notes,
            Status = testDrive.Status,
            CreatedAt = testDrive.CreatedAt,
            UpdatedAt = testDrive.UpdatedAt,
            CancelledAt = testDrive.CancelledAt
        };
    }

    public async Task<TestDriveResponse> UpdateAsync(Guid id, UpdateTestDriveRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var testDrive = await _repository.GetByIdAsync(id, cancellationToken);
        if (testDrive is null) throw new NotFoundException("TestDrive", id);

        if (testDrive.UserId != userId)
            throw new ForbiddenException("You can only update your own test drives.");

        if (testDrive.Status == TestDriveStatus.Completed || testDrive.Status == TestDriveStatus.Cancelled)
            throw new ConflictException($"Cannot update a test drive with status {testDrive.Status}.");

        if (request.RequestedDate.HasValue) testDrive.RequestedDate = request.RequestedDate.Value;
        if (request.StartTime.HasValue) testDrive.StartTime = request.StartTime;
        if (request.EndTime.HasValue) testDrive.EndTime = request.EndTime;
        if (request.LocationId.HasValue) testDrive.LocationId = request.LocationId;
        if (request.Notes != null) testDrive.Notes = request.Notes;

        await _repository.UpdateAsync(testDrive, cancellationToken);

        return new TestDriveResponse
        {
            Id = testDrive.Id,
            UserId = testDrive.UserId,
            ListingId = testDrive.ListingId,
            ListingTitle = string.Empty,
            SellerId = testDrive.SellerId,
            SellerName = null,
            DealerId = testDrive.DealerId,
            DealerName = null,
            RequestedDate = testDrive.RequestedDate,
            StartTime = testDrive.StartTime,
            EndTime = testDrive.EndTime,
            LocationId = testDrive.LocationId,
            LocationName = null,
            Notes = testDrive.Notes,
            Status = testDrive.Status,
            CreatedAt = testDrive.CreatedAt,
            UpdatedAt = testDrive.UpdatedAt,
            CancelledAt = testDrive.CancelledAt
        };
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var testDrive = await _repository.GetByIdAsync(id, cancellationToken);
        if (testDrive is null) throw new NotFoundException("TestDrive", id);

        if (testDrive.UserId != userId)
            throw new ForbiddenException("You can only delete your own test drives.");

        await _repository.DeleteAsync(testDrive, cancellationToken);
    }

    public async Task<TestDriveResponse> ConfirmAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var testDrive = await _repository.GetByIdAsync(id, cancellationToken);
        if (testDrive is null) throw new NotFoundException("TestDrive", id);

        if (testDrive.Status != TestDriveStatus.Requested && testDrive.Status != TestDriveStatus.Pending)
            throw new ConflictException($"Cannot confirm a test drive with status {testDrive.Status}.");

        testDrive.Status = TestDriveStatus.Confirmed;
        await _repository.UpdateAsync(testDrive, cancellationToken);

        return new TestDriveResponse
        {
            Id = testDrive.Id,
            UserId = testDrive.UserId,
            ListingId = testDrive.ListingId,
            ListingTitle = string.Empty,
            SellerId = testDrive.SellerId,
            SellerName = null,
            DealerId = testDrive.DealerId,
            DealerName = null,
            RequestedDate = testDrive.RequestedDate,
            StartTime = testDrive.StartTime,
            EndTime = testDrive.EndTime,
            LocationId = testDrive.LocationId,
            LocationName = null,
            Notes = testDrive.Notes,
            Status = testDrive.Status,
            CreatedAt = testDrive.CreatedAt,
            UpdatedAt = testDrive.UpdatedAt,
            CancelledAt = testDrive.CancelledAt
        };
    }

    public async Task<TestDriveResponse> RescheduleAsync(Guid id, RescheduleTestDriveRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var testDrive = await _repository.GetByIdAsync(id, cancellationToken);
        if (testDrive is null) throw new NotFoundException("TestDrive", id);

        if (testDrive.Status != TestDriveStatus.Confirmed && testDrive.Status != TestDriveStatus.Requested)
            throw new ConflictException($"Cannot reschedule a test drive with status {testDrive.Status}.");

        testDrive.StartTime = request.NewStartTime;
        testDrive.EndTime = request.NewEndTime;
        testDrive.Status = TestDriveStatus.Rescheduled;
        await _repository.UpdateAsync(testDrive, cancellationToken);

        return new TestDriveResponse
        {
            Id = testDrive.Id,
            UserId = testDrive.UserId,
            ListingId = testDrive.ListingId,
            ListingTitle = string.Empty,
            SellerId = testDrive.SellerId,
            SellerName = null,
            DealerId = testDrive.DealerId,
            DealerName = null,
            RequestedDate = testDrive.RequestedDate,
            StartTime = testDrive.StartTime,
            EndTime = testDrive.EndTime,
            LocationId = testDrive.LocationId,
            LocationName = null,
            Notes = testDrive.Notes,
            Status = testDrive.Status,
            CreatedAt = testDrive.CreatedAt,
            UpdatedAt = testDrive.UpdatedAt,
            CancelledAt = testDrive.CancelledAt
        };
    }

    public async Task<TestDriveResponse> CancelAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var testDrive = await _repository.GetByIdAsync(id, cancellationToken);
        if (testDrive is null) throw new NotFoundException("TestDrive", id);

        if (testDrive.Status == TestDriveStatus.Completed || testDrive.Status == TestDriveStatus.Cancelled)
            throw new ConflictException($"Cannot cancel a test drive with status {testDrive.Status}.");

        testDrive.Status = TestDriveStatus.Cancelled;
        testDrive.CancelledAt = DateTime.UtcNow;
        await _repository.UpdateAsync(testDrive, cancellationToken);

        return new TestDriveResponse
        {
            Id = testDrive.Id,
            UserId = testDrive.UserId,
            ListingId = testDrive.ListingId,
            ListingTitle = string.Empty,
            SellerId = testDrive.SellerId,
            SellerName = null,
            DealerId = testDrive.DealerId,
            DealerName = null,
            RequestedDate = testDrive.RequestedDate,
            StartTime = testDrive.StartTime,
            EndTime = testDrive.EndTime,
            LocationId = testDrive.LocationId,
            LocationName = null,
            Notes = testDrive.Notes,
            Status = testDrive.Status,
            CreatedAt = testDrive.CreatedAt,
            UpdatedAt = testDrive.UpdatedAt,
            CancelledAt = testDrive.CancelledAt
        };
    }

    public async Task<TestDriveResponse> CompleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var testDrive = await _repository.GetByIdAsync(id, cancellationToken);
        if (testDrive is null) throw new NotFoundException("TestDrive", id);

        if (testDrive.Status != TestDriveStatus.Confirmed && testDrive.Status != TestDriveStatus.Rescheduled)
            throw new ConflictException($"Cannot complete a test drive with status {testDrive.Status}.");

        testDrive.Status = TestDriveStatus.Completed;
        await _repository.UpdateAsync(testDrive, cancellationToken);

        return new TestDriveResponse
        {
            Id = testDrive.Id,
            UserId = testDrive.UserId,
            ListingId = testDrive.ListingId,
            ListingTitle = string.Empty,
            SellerId = testDrive.SellerId,
            SellerName = null,
            DealerId = testDrive.DealerId,
            DealerName = null,
            RequestedDate = testDrive.RequestedDate,
            StartTime = testDrive.StartTime,
            EndTime = testDrive.EndTime,
            LocationId = testDrive.LocationId,
            LocationName = null,
            Notes = testDrive.Notes,
            Status = testDrive.Status,
            CreatedAt = testDrive.CreatedAt,
            UpdatedAt = testDrive.UpdatedAt,
            CancelledAt = testDrive.CancelledAt
        };
    }

    public async Task<PagedResult<TestDriveResponse>> GetByListingAsync(Guid listingId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(t => t.ListingId == listingId).OrderByDescending(t => t.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TestDriveResponse
            {
                Id = t.Id,
                UserId = t.UserId,
                ListingId = t.ListingId,
                ListingTitle = string.Empty,
                SellerId = t.SellerId,
                SellerName = null,
                DealerId = t.DealerId,
                DealerName = null,
                RequestedDate = t.RequestedDate,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                LocationId = t.LocationId,
                LocationName = null,
                Notes = t.Notes,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                CancelledAt = t.CancelledAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<TestDriveResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<TestDriveResponse>> GetByDealerAsync(Guid dealerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(t => t.DealerId == dealerId).OrderByDescending(t => t.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TestDriveResponse
            {
                Id = t.Id,
                UserId = t.UserId,
                ListingId = t.ListingId,
                ListingTitle = string.Empty,
                SellerId = t.SellerId,
                SellerName = null,
                DealerId = t.DealerId,
                DealerName = null,
                RequestedDate = t.RequestedDate,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                LocationId = t.LocationId,
                LocationName = null,
                Notes = t.Notes,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                CancelledAt = t.CancelledAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<TestDriveResponse>.Create(items, totalCount, page, pageSize);
    }
}
