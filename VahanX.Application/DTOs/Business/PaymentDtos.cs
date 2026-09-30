using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Business;

/// <summary>
/// Payment response DTO.
/// </summary>
public class PaymentResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string PaymentReference { get; set; } = string.Empty;
    public PaymentPurpose PaymentPurpose { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public string? FailureReason { get; set; }
}

/// <summary>
/// Create payment request.
/// </summary>
public class CreatePaymentRequest
{
    public PaymentPurpose PaymentPurpose { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public string Currency { get; set; } = "NPR";
    public string? IdempotencyKey { get; set; }
    public List<PaymentItemRequest> Items { get; set; } = [];
}

/// <summary>
/// Payment item request.
/// </summary>
public class PaymentItemRequest
{
    public string ItemType { get; set; } = string.Empty;
    public string ReferenceId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
}

/// <summary>
/// Invoice response DTO.
/// </summary>
public class InvoiceResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid PaymentId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public InvoiceStatus Status { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime? DueAt { get; set; }
    public DateTime? PaidAt { get; set; }
}

/// <summary>
/// Refund response DTO.
/// </summary>
public class RefundResponse
{
    public Guid Id { get; set; }
    public Guid PaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public RefundStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

/// <summary>
/// Create refund request.
/// </summary>
public class CreateRefundRequest
{
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
}
