using FluentValidation;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Validators.VehicleCore;

/// <summary>
/// Validator for CreateBrandRequest.
/// </summary>
public class CreateBrandValidator : AbstractValidator<CreateBrandRequest>
{
    public CreateBrandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.CountryOfOrigin)
            .MaximumLength(100).WithMessage("CountryOfOrigin cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.CountryOfOrigin));
    }
}

/// <summary>
/// Validator for UpdateBrandRequest.
/// </summary>
public class UpdateBrandValidator : AbstractValidator<UpdateBrandRequest>
{
    public UpdateBrandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.CountryOfOrigin)
            .MaximumLength(100).WithMessage("CountryOfOrigin cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.CountryOfOrigin));
    }
}
