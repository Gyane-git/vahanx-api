using FluentValidation;
using VahanX.Application.DTOs.Services;

namespace VahanX.Application.Validators.Services;

/// <summary>
/// Validator for charging station connector.
/// </summary>
public class ChargingStationConnectorValidator : AbstractValidator<ChargingStationConnectorResponse>
{
    public ChargingStationConnectorValidator()
    {
        RuleFor(x => x.PowerKw)
            .GreaterThan(0).WithMessage("PowerKw must be positive.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be positive.");

        RuleFor(x => x.AvailableQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("AvailableQuantity cannot be negative.")
            .LessThanOrEqualTo(x => x.Quantity).WithMessage("AvailableQuantity cannot exceed Quantity.")
            .When(x => x.AvailableQuantity.HasValue);
    }
}

/// <summary>
/// Validator for charging station price.
/// </summary>
public class ChargingStationPriceValidator : AbstractValidator<ChargingStationPriceResponse>
{
    public ChargingStationPriceValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be positive.");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Unit is required.")
            .MaximumLength(20).WithMessage("Unit cannot exceed 20 characters.");
    }
}
