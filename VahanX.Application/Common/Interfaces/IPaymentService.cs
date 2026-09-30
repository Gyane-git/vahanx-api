using VahanX.Application.Common;
using VahanX.Application.DTOs.Business;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for payment operations.
/// </summary>
public interface IPaymentService
{
    Task<PagedResult<PaymentResponse>> GetPaymentsAsync(int page, int pageSize, Guid? userId = null, Domain.Enums.PaymentStatus? status = null, CancellationToken cancellationToken = default);
    Task<PaymentResponse?> GetPaymentByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<PaymentResponse> ConfirmPaymentAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<PaymentResponse> FailPaymentAsync(Guid id, string reason, Guid userId, CancellationToken cancellationToken = default);
    Task<InvoiceResponse?> GetInvoiceAsync(Guid paymentId, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<InvoiceResponse>> GetInvoicesAsync(int page, int pageSize, Guid userId, CancellationToken cancellationToken = default);
    Task<RefundResponse> CreateRefundAsync(Guid paymentId, CreateRefundRequest request, Guid userId, CancellationToken cancellationToken = default);
}
