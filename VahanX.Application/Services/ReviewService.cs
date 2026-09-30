using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Trust;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of review service.
/// </summary>
public class ReviewService : IReviewService
{
    private readonly IRepository<VehicleReview> _vehicleReviewRepository;
    private readonly IRepository<SellerReview> _sellerReviewRepository;
    private readonly IRepository<DealerReview> _dealerReviewRepository;
    private readonly IRepository<ReviewReport> _reviewReportRepository;
    private readonly IRepository<Vehicle> _vehicleRepository;
    private readonly IRepository<Seller> _sellerRepository;
    private readonly IRepository<Dealer> _dealerRepository;

    public ReviewService(
        IRepository<VehicleReview> vehicleReviewRepository,
        IRepository<SellerReview> sellerReviewRepository,
        IRepository<DealerReview> dealerReviewRepository,
        IRepository<ReviewReport> reviewReportRepository,
        IRepository<Vehicle> vehicleRepository,
        IRepository<Seller> sellerRepository,
        IRepository<Dealer> dealerRepository)
    {
        _vehicleReviewRepository = vehicleReviewRepository;
        _sellerReviewRepository = sellerReviewRepository;
        _dealerReviewRepository = dealerReviewRepository;
        _reviewReportRepository = reviewReportRepository;
        _vehicleRepository = vehicleRepository;
        _sellerRepository = sellerRepository;
        _dealerRepository = dealerRepository;
    }

    public async Task<PagedResult<VehicleReviewResponse>> GetVehicleReviewsAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _vehicleReviewRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(r => r.VehicleId == vehicleId && r.Status == ReviewStatus.Published).OrderByDescending(r => r.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new VehicleReviewResponse
            {
                Id = r.Id,
                UserId = r.UserId,
                VehicleId = r.VehicleId,
                ListingId = r.ListingId,
                Rating = r.Rating,
                Title = r.Title,
                Comment = r.Comment,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<VehicleReviewResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<SellerReviewResponse>> GetSellerReviewsAsync(Guid sellerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _sellerReviewRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(r => r.SellerId == sellerId && r.Status == ReviewStatus.Published).OrderByDescending(r => r.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new SellerReviewResponse
            {
                Id = r.Id,
                UserId = r.UserId,
                SellerId = r.SellerId,
                Rating = r.Rating,
                Title = r.Title,
                Comment = r.Comment,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<SellerReviewResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<DealerReviewResponse>> GetDealerReviewsAsync(Guid dealerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _dealerReviewRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(r => r.DealerId == dealerId && r.Status == ReviewStatus.Published).OrderByDescending(r => r.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new DealerReviewResponse
            {
                Id = r.Id,
                UserId = r.UserId,
                DealerId = r.DealerId,
                Rating = r.Rating,
                Title = r.Title,
                Comment = r.Comment,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<DealerReviewResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<VehicleReviewResponse?> GetVehicleReviewByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var review = await _vehicleReviewRepository.GetByIdAsync(id, cancellationToken);

        if (review is null)
            return null;

        return new VehicleReviewResponse
        {
            Id = review.Id,
            UserId = review.UserId,
            VehicleId = review.VehicleId,
            ListingId = review.ListingId,
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            Status = review.Status,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt
        };
    }

    public async Task<VehicleReviewResponse> CreateVehicleReviewAsync(Guid vehicleId, Guid userId, CreateReviewRequest request, CancellationToken cancellationToken = default)
    {
        var vehicleExists = await _vehicleRepository.AnyAsync(v => v.Id == vehicleId, cancellationToken);
        if (!vehicleExists) throw new NotFoundException("Vehicle", vehicleId);

        var existingReview = await _vehicleReviewRepository.AnyAsync(r => r.VehicleId == vehicleId && r.UserId == userId, cancellationToken);
        if (existingReview) throw new ConflictException("User has already reviewed this vehicle.");

        var review = new VehicleReview
        {
            UserId = userId,
            VehicleId = vehicleId,
            Rating = request.Rating,
            Title = request.Title,
            Comment = request.Comment,
            Status = ReviewStatus.Pending
        };

        await _vehicleReviewRepository.AddAsync(review, cancellationToken);

        return new VehicleReviewResponse
        {
            Id = review.Id,
            UserId = review.UserId,
            VehicleId = review.VehicleId,
            ListingId = review.ListingId,
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            Status = review.Status,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt
        };
    }

    public async Task<SellerReviewResponse> CreateSellerReviewAsync(Guid sellerId, Guid userId, CreateReviewRequest request, CancellationToken cancellationToken = default)
    {
        var sellerExists = await _sellerRepository.AnyAsync(s => s.Id == sellerId, cancellationToken);
        if (!sellerExists) throw new NotFoundException("Seller", sellerId);

        var seller = await _sellerRepository.GetByIdAsync(sellerId, cancellationToken);
        if (seller != null && seller.UserId == userId)
            throw new ConflictException("Cannot review your own seller profile.");

        var existingReview = await _sellerReviewRepository.AnyAsync(r => r.SellerId == sellerId && r.UserId == userId, cancellationToken);
        if (existingReview) throw new ConflictException("User has already reviewed this seller.");

        var review = new SellerReview
        {
            UserId = userId,
            SellerId = sellerId,
            Rating = request.Rating,
            Title = request.Title,
            Comment = request.Comment,
            Status = ReviewStatus.Pending
        };

        await _sellerReviewRepository.AddAsync(review, cancellationToken);

        return new SellerReviewResponse
        {
            Id = review.Id,
            UserId = review.UserId,
            SellerId = review.SellerId,
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            Status = review.Status,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt
        };
    }

    public async Task<DealerReviewResponse> CreateDealerReviewAsync(Guid dealerId, Guid userId, CreateReviewRequest request, CancellationToken cancellationToken = default)
    {
        var dealerExists = await _dealerRepository.AnyAsync(d => d.Id == dealerId, cancellationToken);
        if (!dealerExists) throw new NotFoundException("Dealer", dealerId);

        var existingReview = await _dealerReviewRepository.AnyAsync(r => r.DealerId == dealerId && r.UserId == userId, cancellationToken);
        if (existingReview) throw new ConflictException("User has already reviewed this dealer.");

        var review = new DealerReview
        {
            UserId = userId,
            DealerId = dealerId,
            Rating = request.Rating,
            Title = request.Title,
            Comment = request.Comment,
            Status = ReviewStatus.Pending
        };

        await _dealerReviewRepository.AddAsync(review, cancellationToken);

        return new DealerReviewResponse
        {
            Id = review.Id,
            UserId = review.UserId,
            DealerId = review.DealerId,
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            Status = review.Status,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt
        };
    }

    public async Task<VehicleReviewResponse> UpdateVehicleReviewAsync(Guid id, Guid userId, UpdateReviewRequest request, CancellationToken cancellationToken = default)
    {
        var review = await _vehicleReviewRepository.GetByIdAsync(id, cancellationToken);
        if (review is null) throw new NotFoundException("Review", id);

        if (review.UserId != userId)
            throw new ForbiddenException("You can only update your own reviews.");

        if (review.Status == ReviewStatus.Rejected || review.Status == ReviewStatus.Hidden)
            throw new ConflictException($"Cannot update a review with status {review.Status}.");

        if (request.Rating.HasValue)
            review.Rating = request.Rating.Value;
        if (request.Title != null)
            review.Title = request.Title;
        if (request.Comment != null)
            review.Comment = request.Comment;

        await _vehicleReviewRepository.UpdateAsync(review, cancellationToken);

        return new VehicleReviewResponse
        {
            Id = review.Id,
            UserId = review.UserId,
            VehicleId = review.VehicleId,
            ListingId = review.ListingId,
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            Status = review.Status,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt
        };
    }

    public async Task DeleteReviewAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var review = await _vehicleReviewRepository.GetByIdAsync(id, cancellationToken);
        if (review is null) throw new NotFoundException("Review", id);

        if (review.UserId != userId)
            throw new ForbiddenException("You can only delete your own reviews.");

        await _vehicleReviewRepository.DeleteAsync(review, cancellationToken);
    }

    public async Task ReportReviewAsync(Guid id, Guid userId, ReportReviewRequest request, CancellationToken cancellationToken = default)
    {
        var review = await _vehicleReviewRepository.GetByIdAsync(id, cancellationToken);
        if (review is null) throw new NotFoundException("Review", id);

        var report = new ReviewReport
        {
            ReviewId = id,
            ReportedBy = userId,
            Reason = request.Reason,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _reviewReportRepository.AddAsync(report, cancellationToken);
    }

    public async Task<RatingSummaryResponse> GetVehicleRatingSummaryAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var reviews = await _vehicleReviewRepository.QueryAsync(true, cancellationToken);
        var publishedReviews = await reviews
            .Where(r => r.VehicleId == vehicleId && r.Status == ReviewStatus.Published)
            .ToListAsync(cancellationToken);

        return CalculateRatingSummary(publishedReviews.Select(r => r.Rating));
    }

    public async Task<RatingSummaryResponse> GetSellerRatingSummaryAsync(Guid sellerId, CancellationToken cancellationToken = default)
    {
        var reviews = await _sellerReviewRepository.QueryAsync(true, cancellationToken);
        var publishedReviews = await reviews
            .Where(r => r.SellerId == sellerId && r.Status == ReviewStatus.Published)
            .ToListAsync(cancellationToken);

        return CalculateRatingSummary(publishedReviews.Select(r => r.Rating));
    }

    public async Task<RatingSummaryResponse> GetDealerRatingSummaryAsync(Guid dealerId, CancellationToken cancellationToken = default)
    {
        var reviews = await _dealerReviewRepository.QueryAsync(true, cancellationToken);
        var publishedReviews = await reviews
            .Where(r => r.DealerId == dealerId && r.Status == ReviewStatus.Published)
            .ToListAsync(cancellationToken);

        return CalculateRatingSummary(publishedReviews.Select(r => r.Rating));
    }

    private static RatingSummaryResponse CalculateRatingSummary(IEnumerable<int> ratings)
    {
        var ratingList = ratings.ToList();
        var total = ratingList.Count;

        return new RatingSummaryResponse
        {
            AverageRating = total > 0 ? (decimal)ratingList.Average() : 0,
            TotalReviews = total,
            Rating1Count = ratingList.Count(r => r == 1),
            Rating2Count = ratingList.Count(r => r == 2),
            Rating3Count = ratingList.Count(r => r == 3),
            Rating4Count = ratingList.Count(r => r == 4),
            Rating5Count = ratingList.Count(r => r == 5)
        };
    }
}
