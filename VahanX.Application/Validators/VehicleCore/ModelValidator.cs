using FluentValidation;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Validators.VehicleCore;

/// <summary>
/// Validator for CreateModelRequest.
/// </summary>
public class CreateModelValidator : AbstractValidator<CreateModelRequest>
{
    public CreateModelValidator()
    {
        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("BrandId is required.");

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
    }
}

/// <summary>
/// Validator for UpdateModelRequest.
/// </summary>
public class UpdateModelValidator : AbstractValidator<UpdateModelRequest>
{
    public UpdateModelValidator()
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
    }
}
