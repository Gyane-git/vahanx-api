using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Business;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for payment operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _service;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService service, ILogger<PaymentsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all payments.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PaymentResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<PaymentResponse>>>> GetPayments(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? userId = null,
        [FromQuery] Domain.Enums.PaymentStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPaymentsAsync(page, pageSize, userId, status, cancellationToken);
        return Ok(ApiResponse<PagedResult<PaymentResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get payment by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> GetPaymentById(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPaymentByIdAsync(id, userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Payment not found."));

        return Ok(ApiResponse<PaymentResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new payment.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> CreatePayment(
        [FromBody] CreatePaymentRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreatePaymentAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetPaymentById), new { id = result.Id, userId }, ApiResponse<PaymentResponse>.SuccessResponse(result, "Payment created successfully."));
    }

    /// <summary>
    /// Confirm a payment.
    /// </summary>
    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> ConfirmPayment(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.ConfirmPaymentAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<PaymentResponse>.SuccessResponse(result, "Payment confirmed successfully."));
    }

    /// <summary>
    /// Fail a payment.
    /// </summary>
    [HttpPost("{id:guid}/fail")]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> FailPayment(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] FailPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.FailPaymentAsync(id, request.Reason, userId, cancellationToken);
        return Ok(ApiResponse<PaymentResponse>.SuccessResponse(result, "Payment marked as failed."));
    }

    /// <summary>
    /// Get invoice for a payment.
    /// </summary>
    [HttpGet("{id:guid}/invoice")]
    [ProducesResponseType(typeof(ApiResponse<InvoiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<InvoiceResponse>>> GetInvoice(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetInvoiceAsync(id, userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Invoice not found."));

        return Ok(ApiResponse<InvoiceResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Get all invoices for a user.
    /// </summary>
    [HttpGet("invoices")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<InvoiceResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<InvoiceResponse>>>> GetInvoices(
        [FromQuery] Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetInvoicesAsync(page, pageSize, userId, cancellationToken);
        return Ok(ApiResponse<PagedResult<InvoiceResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a refund for a payment.
    /// </summary>
    [HttpPost("{id:guid}/refund")]
    [ProducesResponseType(typeof(ApiResponse<RefundResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<RefundResponse>>> CreateRefund(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] CreateRefundRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateRefundAsync(id, request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetPaymentById), new { id, userId }, ApiResponse<RefundResponse>.SuccessResponse(result, "Refund created successfully."));
    }
}

/// <summary>
/// Request to fail a payment.
/// </summary>
public class FailPaymentRequest
{
    public string Reason { get; set; } = string.Empty;
}
