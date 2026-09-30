using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Business;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of payment service.
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IRepository<PaymentItem> _itemRepository;
    private readonly IRepository<PaymentTransaction> _transactionRepository;
    private readonly IRepository<PaymentRefund> _refundRepository;
    private readonly IRepository<Invoice> _invoiceRepository;

    public PaymentService(
        IRepository<Payment> paymentRepository,
        IRepository<PaymentItem> itemRepository,
        IRepository<PaymentTransaction> transactionRepository,
        IRepository<PaymentRefund> refundRepository,
        IRepository<Invoice> invoiceRepository)
    {
        _paymentRepository = paymentRepository;
        _itemRepository = itemRepository;
        _transactionRepository = transactionRepository;
        _refundRepository = refundRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<PagedResult<PaymentResponse>> GetPaymentsAsync(int page, int pageSize, Guid? userId = null, PaymentStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = await _paymentRepository.QueryAsync(true, cancellationToken);

        if (userId.HasValue)
            query = query.Where(p => p.UserId == userId.Value);
        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PaymentResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                PaymentReference = p.PaymentReference,
                PaymentPurpose = p.PaymentPurpose,
                Status = p.Status,
                Amount = p.Amount,
                TaxAmount = p.TaxAmount,
                DiscountAmount = p.DiscountAmount,
                TotalAmount = p.TotalAmount,
                Currency = p.Currency,
                Provider = p.Provider,
                CreatedAt = p.CreatedAt,
                CompletedAt = p.CompletedAt,
                FailedAt = p.FailedAt,
                FailureReason = p.FailureReason
            })
            .ToListAsync(cancellationToken);

        return PagedResult<PaymentResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PaymentResponse?> GetPaymentByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        if (payment is null) return null;

        if (payment.UserId != userId)
            throw new ForbiddenException("You do not have access to this payment.");

        return new PaymentResponse
        {
            Id = payment.Id,
            UserId = payment.UserId,
            PaymentReference = payment.PaymentReference,
            PaymentPurpose = payment.PaymentPurpose,
            Status = payment.Status,
            Amount = payment.Amount,
            TaxAmount = payment.TaxAmount,
            DiscountAmount = payment.DiscountAmount,
            TotalAmount = payment.TotalAmount,
            Currency = payment.Currency,
            Provider = payment.Provider,
            CreatedAt = payment.CreatedAt,
            CompletedAt = payment.CompletedAt,
            FailedAt = payment.FailedAt,
            FailureReason = payment.FailureReason
        };
    }

    public async Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var totalAmount = request.Amount + request.TaxAmount - request.DiscountAmount;

        var payment = new Payment
        {
            UserId = userId,
            PaymentReference = GeneratePaymentReference(),
            PaymentPurpose = request.PaymentPurpose,
            Status = PaymentStatus.Pending,
            Amount = request.Amount,
            TaxAmount = request.TaxAmount,
            DiscountAmount = request.DiscountAmount,
            TotalAmount = totalAmount,
            Currency = request.Currency,
            IdempotencyKey = request.IdempotencyKey
        };

        await _paymentRepository.AddAsync(payment, cancellationToken);

        foreach (var item in request.Items)
        {
            var paymentItem = new PaymentItem
            {
                PaymentId = payment.Id,
                ItemType = item.ItemType,
                ReferenceId = item.ReferenceId,
                Description = item.Description,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice
            };

            await _itemRepository.AddAsync(paymentItem, cancellationToken);
        }

        return new PaymentResponse
        {
            Id = payment.Id,
            UserId = payment.UserId,
            PaymentReference = payment.PaymentReference,
            PaymentPurpose = payment.PaymentPurpose,
            Status = payment.Status,
            Amount = payment.Amount,
            TaxAmount = payment.TaxAmount,
            DiscountAmount = payment.DiscountAmount,
            TotalAmount = payment.TotalAmount,
            Currency = payment.Currency,
            Provider = payment.Provider,
            CreatedAt = payment.CreatedAt,
            CompletedAt = payment.CompletedAt,
            FailedAt = payment.FailedAt,
            FailureReason = payment.FailureReason
        };
    }

    public async Task<PaymentResponse> ConfirmPaymentAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        if (payment is null) throw new NotFoundException("Payment", id);

        if (payment.UserId != userId)
            throw new ForbiddenException("You can only confirm your own payments.");

        if (payment.Status != PaymentStatus.Pending && payment.Status != PaymentStatus.Processing)
            throw new ConflictException($"Cannot confirm a payment with status {payment.Status}.");

        payment.Status = PaymentStatus.Succeeded;
        payment.CompletedAt = DateTime.UtcNow;
        await _paymentRepository.UpdateAsync(payment, cancellationToken);

        var transaction = new PaymentTransaction
        {
            PaymentId = payment.Id,
            Provider = "Mock",
            TransactionType = PaymentTransactionType.Payment,
            ProviderTransactionReference = $"TXN-{Guid.NewGuid():N}",
            Amount = payment.TotalAmount,
            Currency = payment.Currency,
            Status = "Succeeded",
            ProcessedAt = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction, cancellationToken);

        return new PaymentResponse
        {
            Id = payment.Id,
            UserId = payment.UserId,
            PaymentReference = payment.PaymentReference,
            PaymentPurpose = payment.PaymentPurpose,
            Status = payment.Status,
            Amount = payment.Amount,
            TaxAmount = payment.TaxAmount,
            DiscountAmount = payment.DiscountAmount,
            TotalAmount = payment.TotalAmount,
            Currency = payment.Currency,
            Provider = payment.Provider,
            CreatedAt = payment.CreatedAt,
            CompletedAt = payment.CompletedAt,
            FailedAt = payment.FailedAt,
            FailureReason = payment.FailureReason
        };
    }

    public async Task<PaymentResponse> FailPaymentAsync(Guid id, string reason, Guid userId, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        if (payment is null) throw new NotFoundException("Payment", id);

        if (payment.UserId != userId)
            throw new ForbiddenException("You can only fail your own payments.");

        if (payment.Status != PaymentStatus.Pending && payment.Status != PaymentStatus.Processing)
            throw new ConflictException($"Cannot fail a payment with status {payment.Status}.");

        payment.Status = PaymentStatus.Failed;
        payment.FailedAt = DateTime.UtcNow;
        payment.FailureReason = reason;
        await _paymentRepository.UpdateAsync(payment, cancellationToken);

        return new PaymentResponse
        {
            Id = payment.Id,
            UserId = payment.UserId,
            PaymentReference = payment.PaymentReference,
            PaymentPurpose = payment.PaymentPurpose,
            Status = payment.Status,
            Amount = payment.Amount,
            TaxAmount = payment.TaxAmount,
            DiscountAmount = payment.DiscountAmount,
            TotalAmount = payment.TotalAmount,
            Currency = payment.Currency,
            Provider = payment.Provider,
            CreatedAt = payment.CreatedAt,
            CompletedAt = payment.CompletedAt,
            FailedAt = payment.FailedAt,
            FailureReason = payment.FailureReason
        };
    }

    public async Task<InvoiceResponse?> GetInvoiceAsync(Guid paymentId, Guid userId, CancellationToken cancellationToken = default)
    {
        var invoices = await _invoiceRepository.QueryAsync(true, cancellationToken);
        var invoice = await invoices
            .FirstOrDefaultAsync(i => i.PaymentId == paymentId, cancellationToken);

        if (invoice is null) return null;

        if (invoice.UserId != userId)
            throw new ForbiddenException("You do not have access to this invoice.");

        return new InvoiceResponse
        {
            Id = invoice.Id,
            UserId = invoice.UserId,
            InvoiceNumber = invoice.InvoiceNumber,
            PaymentId = invoice.PaymentId,
            Subtotal = invoice.Subtotal,
            TaxAmount = invoice.TaxAmount,
            DiscountAmount = invoice.DiscountAmount,
            TotalAmount = invoice.TotalAmount,
            Currency = invoice.Currency,
            Status = invoice.Status,
            IssuedAt = invoice.IssuedAt,
            DueAt = invoice.DueAt,
            PaidAt = invoice.PaidAt
        };
    }

    public async Task<PagedResult<InvoiceResponse>> GetInvoicesAsync(int page, int pageSize, Guid userId, CancellationToken cancellationToken = default)
    {
        var query = await _invoiceRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(i => i.UserId == userId).OrderByDescending(i => i.IssuedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new InvoiceResponse
            {
                Id = i.Id,
                UserId = i.UserId,
                InvoiceNumber = i.InvoiceNumber,
                PaymentId = i.PaymentId,
                Subtotal = i.Subtotal,
                TaxAmount = i.TaxAmount,
                DiscountAmount = i.DiscountAmount,
                TotalAmount = i.TotalAmount,
                Currency = i.Currency,
                Status = i.Status,
                IssuedAt = i.IssuedAt,
                DueAt = i.DueAt,
                PaidAt = i.PaidAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<InvoiceResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<RefundResponse> CreateRefundAsync(Guid paymentId, CreateRefundRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        if (payment is null) throw new NotFoundException("Payment", paymentId);

        if (payment.UserId != userId)
            throw new ForbiddenException("You can only refund your own payments.");

        if (payment.Status != PaymentStatus.Succeeded)
            throw new ConflictException($"Cannot refund a payment with status {payment.Status}.");

        var refund = new PaymentRefund
        {
            PaymentId = paymentId,
            Amount = request.Amount,
            Currency = payment.Currency,
            Reason = request.Reason,
            Status = RefundStatus.Requested
        };

        await _refundRepository.AddAsync(refund, cancellationToken);

        return new RefundResponse
        {
            Id = refund.Id,
            PaymentId = refund.PaymentId,
            Amount = refund.Amount,
            Currency = refund.Currency,
            Reason = refund.Reason,
            Status = refund.Status,
            CreatedAt = refund.CreatedAt,
            CompletedAt = refund.CompletedAt
        };
    }

    private static string GeneratePaymentReference()
    {
        return $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
    }
}
