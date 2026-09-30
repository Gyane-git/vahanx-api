using FluentValidation;
using VahanX.Application.DTOs.Business;

namespace VahanX.Application.Validators.Business;

/// <summary>
/// Validator for CreatePaymentRequest.
/// </summary>
public class CreatePaymentValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.PaymentPurpose)
            .IsInEnum().WithMessage("Invalid payment purpose.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be positive.");

        RuleFor(x => x.TaxAmount)
            .GreaterThanOrEqualTo(0).WithMessage("TaxAmount must be non-negative.");

        RuleFor(x => x.DiscountAmount)
            .GreaterThanOrEqualTo(0).WithMessage("DiscountAmount must be non-negative.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required.");

        RuleForEach(x => x.Items).SetValidator(new PaymentItemValidator());
    }
}

/// <summary>
/// Validator for PaymentItemRequest.
/// </summary>
public class PaymentItemValidator : AbstractValidator<PaymentItemRequest>
{
    public PaymentItemValidator()
    {
        RuleFor(x => x.ItemType)
            .NotEmpty().WithMessage("ItemType is required.");

        RuleFor(x => x.ReferenceId)
            .NotEmpty().WithMessage("ReferenceId is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be positive.");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be non-negative.");
    }
}

/// <summary>
/// Validator for CreateRefundRequest.
/// </summary>
public class CreateRefundValidator : AbstractValidator<CreateRefundRequest>
{
    public CreateRefundValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be positive.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(1000).WithMessage("Reason cannot exceed 1000 characters.");
    }
}
