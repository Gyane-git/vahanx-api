using FluentValidation;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Validators.VehicleCore;

/// <summary>
/// Validator for CreateGenerationRequest.
/// </summary>
public class CreateGenerationValidator : AbstractValidator<CreateGenerationRequest>
{
    public CreateGenerationValidator()
    {
        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("ModelId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.StartYear)
            .InclusiveBetween(1900, 2100).WithMessage("StartYear must be between 1900 and 2100.");

        RuleFor(x => x.EndYear)
            .InclusiveBetween(1900, 2100).WithMessage("EndYear must be between 1900 and 2100.")
            .When(x => x.EndYear.HasValue);

        RuleFor(x => x.EndYear)
            .GreaterThanOrEqualTo(x => x.StartYear)
            .WithMessage("EndYear cannot be less than StartYear.")
            .When(x => x.EndYear.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for UpdateGenerationRequest.
/// </summary>
public class UpdateGenerationValidator : AbstractValidator<UpdateGenerationRequest>
{
    public UpdateGenerationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.StartYear)
            .InclusiveBetween(1900, 2100).WithMessage("StartYear must be between 1900 and 2100.");

        RuleFor(x => x.EndYear)
            .InclusiveBetween(1900, 2100).WithMessage("EndYear must be between 1900 and 2100.")
            .When(x => x.EndYear.HasValue);

        RuleFor(x => x.EndYear)
            .GreaterThanOrEqualTo(x => x.StartYear)
            .WithMessage("EndYear cannot be less than StartYear.")
            .When(x => x.EndYear.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
