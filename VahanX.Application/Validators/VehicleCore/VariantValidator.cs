using FluentValidation;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Validators.VehicleCore;

/// <summary>
/// Validator for CreateVariantRequest.
/// </summary>
public class CreateVariantValidator : AbstractValidator<CreateVariantRequest>
{
    public CreateVariantValidator()
    {
        RuleFor(x => x.GenerationId)
            .NotEmpty().WithMessage("GenerationId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.ModelYearFrom)
            .InclusiveBetween(1900, 2100).WithMessage("ModelYearFrom must be between 1900 and 2100.");

        RuleFor(x => x.ModelYearTo)
            .InclusiveBetween(1900, 2100).WithMessage("ModelYearTo must be between 1900 and 2100.")
            .When(x => x.ModelYearTo.HasValue);

        RuleFor(x => x.ModelYearTo)
            .GreaterThanOrEqualTo(x => x.ModelYearFrom)
            .WithMessage("ModelYearTo cannot be less than ModelYearFrom.")
            .When(x => x.ModelYearTo.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for UpdateVariantRequest.
/// </summary>
public class UpdateVariantValidator : AbstractValidator<UpdateVariantRequest>
{
    public UpdateVariantValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.ModelYearFrom)
            .InclusiveBetween(1900, 2100).WithMessage("ModelYearFrom must be between 1900 and 2100.");

        RuleFor(x => x.ModelYearTo)
            .InclusiveBetween(1900, 2100).WithMessage("ModelYearTo must be between 1900 and 2100.")
            .When(x => x.ModelYearTo.HasValue);

        RuleFor(x => x.ModelYearTo)
            .GreaterThanOrEqualTo(x => x.ModelYearFrom)
            .WithMessage("ModelYearTo cannot be less than ModelYearFrom.")
            .When(x => x.ModelYearTo.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
