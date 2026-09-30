using FluentValidation;
using VahanX.Application.DTOs.Services;

namespace VahanX.Application.Validators.Services;

/// <summary>
/// Validator for fuel station price.
/// </summary>
public class FuelStationPriceValidator : AbstractValidator<FuelStationPriceResponse>
{
    public FuelStationPriceValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be positive.");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Unit is required.")
            .MaximumLength(20).WithMessage("Unit cannot exceed 20 characters.");
    }
}
