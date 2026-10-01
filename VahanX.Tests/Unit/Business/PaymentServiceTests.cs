using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.Business;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Business;

/// <summary>
/// Unit tests for PaymentService.
/// </summary>
public class PaymentServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly PaymentService _service;

    public PaymentServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var paymentRepository = new Repository<Payment>(_context);
        var itemRepository = new Repository<PaymentItem>(_context);
        var transactionRepository = new Repository<PaymentTransaction>(_context);
        var refundRepository = new Repository<PaymentRefund>(_context);
        var invoiceRepository = new Repository<Invoice>(_context);

        _service = new PaymentService(paymentRepository, itemRepository, transactionRepository, refundRepository, invoiceRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreatePaymentAsync_WithValidRequest_CreatesPendingPayment()
    {
        // Arrange
        var request = new CreatePaymentRequest
        {
            PaymentPurpose = PaymentPurpose.Subscription,
            Amount = 999,
            TaxAmount = 0,
            DiscountAmount = 0,
            Items = new List<PaymentItemRequest>
            {
                new() { ItemType = "SubscriptionPlan", ReferenceId = Guid.NewGuid().ToString(), Description = "Basic Plan", Quantity = 1, UnitPrice = 999 }
            }
        };

        // Act
        var result = await _service.CreatePaymentAsync(request, Guid.NewGuid());

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(PaymentStatus.Pending, result.Status);
        Assert.Equal(999m, result.TotalAmount);
    }

    [Fact]
    public async Task ConfirmPaymentAsync_WithPendingStatus_ChangesToSucceeded()
    {
        // Arrange
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            PaymentReference = "PAY-TEST-001",
            PaymentPurpose = PaymentPurpose.Subscription,
            Status = PaymentStatus.Pending,
            Amount = 999,
            TaxAmount = 0,
            DiscountAmount = 0,
            TotalAmount = 999
        };
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ConfirmPaymentAsync(payment.Id, payment.UserId);

        // Assert
        Assert.Equal(PaymentStatus.Succeeded, result.Status);
        Assert.NotNull(result.CompletedAt);
    }

    [Fact]
    public async Task ConfirmPaymentAsync_WithDifferentUser_ThrowsForbiddenException()
    {
        // Arrange
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            PaymentReference = "PAY-TEST-002",
            PaymentPurpose = PaymentPurpose.Subscription,
            Status = PaymentStatus.Pending,
            Amount = 999,
            TaxAmount = 0,
            DiscountAmount = 0,
            TotalAmount = 999
        };
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _service.ConfirmPaymentAsync(payment.Id, Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateRefundAsync_WithSucceededPayment_CreatesRefund()
    {
        // Arrange
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            PaymentReference = "PAY-TEST-003",
            PaymentPurpose = PaymentPurpose.Subscription,
            Status = PaymentStatus.Succeeded,
            Amount = 999,
            TaxAmount = 0,
            DiscountAmount = 0,
            TotalAmount = 999
        };
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        var request = new CreateRefundRequest
        {
            Amount = 500,
            Reason = "Partial refund"
        };

        // Act
        var result = await _service.CreateRefundAsync(payment.Id, request, payment.UserId);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(500m, result.Amount);
        Assert.Equal(RefundStatus.Requested, result.Status);
    }
}
