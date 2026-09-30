using VahanX.Application.Common;
using VahanX.Application.DTOs.Business;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for sell vehicle operations.
/// </summary>
public interface ISellVehicleService
{
    Task<PagedResult<SellRequestResponse>> GetRequestsAsync(int page, int pageSize, Guid? userId = null, Domain.Enums.SellRequestStatus? status = null, CancellationToken cancellationToken = default);
    Task<SellRequestResponse?> GetRequestByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<SellRequestResponse> CreateRequestAsync(CreateSellRequestRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<SellRequestResponse> UpdateRequestAsync(Guid id, UpdateSellRequestRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<SellRequestResponse> SubmitRequestAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<SellRequestResponse> CancelRequestAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<SellVehicleResponse> CreateSellVehicleAsync(Guid requestId, CreateSellVehicleRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<SellVehicleResponse?> GetSellVehicleAsync(Guid requestId, Guid userId, CancellationToken cancellationToken = default);
    Task<SellOfferResponse> CreateOfferAsync(Guid requestId, CreateSellOfferRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<SellOfferResponse> AcceptOfferAsync(Guid offerId, Guid userId, CancellationToken cancellationToken = default);
    Task<SellOfferResponse> RejectOfferAsync(Guid offerId, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<SellOfferResponse>> GetRequestOffersAsync(Guid requestId, Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
}
