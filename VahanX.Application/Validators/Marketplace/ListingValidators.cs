using FluentValidation;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Application.Validators.Marketplace;

/// <summary>
/// Validator for CreateListingRequest.
/// </summary>
public class CreateListingValidator : AbstractValidator<CreateListingRequest>
{
    public CreateListingValidator()
    {
        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("VehicleId is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(300).WithMessage("Title cannot exceed 300 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0).WithMessage("Mileage cannot be negative.");

        RuleFor(x => x.ManufactureYear)
            .InclusiveBetween(1900, 2100).WithMessage("ManufactureYear must be between 1900 and 2100.");

        RuleFor(x => x.RegistrationYear)
            .InclusiveBetween(1900, 2100).WithMessage("RegistrationYear must be between 1900 and 2100.")
            .When(x => x.RegistrationYear.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x)
            .Must(x => x.SellerId.HasValue || x.DealerId.HasValue)
            .WithMessage("Either SellerId or DealerId must be specified.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-character code.");

        RuleFor(x => x.MileageUnit)
            .NotEmpty().WithMessage("MileageUnit is required.");
    }
}

/// <summary>
/// Validator for UpdateListingRequest.
/// </summary>
public class UpdateListingValidator : AbstractValidator<UpdateListingRequest>
{
    public UpdateListingValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(300).WithMessage("Title cannot exceed 300 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0).WithMessage("Mileage cannot be negative.");

        RuleFor(x => x.ManufactureYear)
            .InclusiveBetween(1900, 2100).WithMessage("ManufactureYear must be between 1900 and 2100.");

        RuleFor(x => x.RegistrationYear)
            .InclusiveBetween(1900, 2100).WithMessage("RegistrationYear must be between 1900 and 2100.")
            .When(x => x.RegistrationYear.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-character code.");

        RuleFor(x => x.MileageUnit)
            .NotEmpty().WithMessage("MileageUnit is required.");
    }
}
