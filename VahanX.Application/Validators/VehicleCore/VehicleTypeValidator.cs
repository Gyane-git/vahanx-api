using FluentValidation;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Validators.VehicleCore;

/// <summary>
/// Validator for CreateVehicleTypeRequest.
/// </summary>
public class CreateVehicleTypeValidator : AbstractValidator<CreateVehicleTypeRequest>
{
    public CreateVehicleTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for UpdateVehicleTypeRequest.
/// </summary>
public class UpdateVehicleTypeValidator : AbstractValidator<UpdateVehicleTypeRequest>
{
    public UpdateVehicleTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
