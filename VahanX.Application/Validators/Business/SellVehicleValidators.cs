using FluentValidation;
using VahanX.Application.DTOs.Business;

namespace VahanX.Application.Validators.Business;

/// <summary>
/// Validator for CreateSellRequestRequest.
/// </summary>
public class CreateSellRequestValidator : AbstractValidator<CreateSellRequestRequest>
{
    public CreateSellRequestValidator()
    {
        RuleFor(x => x.PreferredContactTime)
            .MaximumLength(100).WithMessage("PreferredContactTime cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.PreferredContactTime));

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

/// <summary>
/// Validator for CreateSellVehicleRequest.
/// </summary>
public class CreateSellVehicleValidator : AbstractValidator<CreateSellVehicleRequest>
{
    public CreateSellVehicleValidator()
    {
        RuleFor(x => x.VehicleTypeId)
            .NotEmpty().WithMessage("VehicleTypeId is required.");

        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("BrandId is required.");

        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("ModelId is required.");

        RuleFor(x => x.ManufactureYear)
            .InclusiveBetween(1900, 2100).WithMessage("ManufactureYear must be between 1900 and 2100.");

        RuleFor(x => x.RegistrationYear)
            .InclusiveBetween(1900, 2100).WithMessage("RegistrationYear must be between 1900 and 2100.")
            .When(x => x.RegistrationYear.HasValue);

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0).WithMessage("Mileage cannot be negative.");

        RuleFor(x => x.FuelTypeId)
            .NotEmpty().WithMessage("FuelTypeId is required.");

        RuleFor(x => x.AskingPrice)
            .GreaterThanOrEqualTo(0).WithMessage("AskingPrice must be non-negative.")
            .When(x => x.AskingPrice.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for CreateSellOfferRequest.
/// </summary>
public class CreateSellOfferValidator : AbstractValidator<CreateSellOfferRequest>
{
    public CreateSellOfferValidator()
    {
        RuleFor(x => x.OfferedAmount)
            .GreaterThan(0).WithMessage("OfferedAmount must be positive.");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
