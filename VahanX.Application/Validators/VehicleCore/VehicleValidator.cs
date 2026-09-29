using FluentValidation;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Validators.VehicleCore;

/// <summary>
/// Validator for CreateVehicleRequest.
/// </summary>
public class CreateVehicleValidator : AbstractValidator<CreateVehicleRequest>
{
    public CreateVehicleValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty().WithMessage("VariantId is required.");

        RuleFor(x => x.BodyTypeId)
            .NotEmpty().WithMessage("BodyTypeId is required.");

        RuleFor(x => x.FuelTypeId)
            .NotEmpty().WithMessage("FuelTypeId is required.");

        RuleFor(x => x.TransmissionTypeId)
            .NotEmpty().WithMessage("TransmissionTypeId is required.");

        RuleFor(x => x.DriveTypeId)
            .NotEmpty().WithMessage("DriveTypeId is required.");

        RuleFor(x => x.EngineTypeId)
            .NotEmpty().WithMessage("EngineTypeId is required.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for UpdateVehicleRequest.
/// </summary>
public class UpdateVehicleValidator : AbstractValidator<UpdateVehicleRequest>
{
    public UpdateVehicleValidator()
    {
        RuleFor(x => x.BodyTypeId)
            .NotEmpty().WithMessage("BodyTypeId is required.");

        RuleFor(x => x.FuelTypeId)
            .NotEmpty().WithMessage("FuelTypeId is required.");

        RuleFor(x => x.TransmissionTypeId)
            .NotEmpty().WithMessage("TransmissionTypeId is required.");

        RuleFor(x => x.DriveTypeId)
            .NotEmpty().WithMessage("DriveTypeId is required.");

        RuleFor(x => x.EngineTypeId)
            .NotEmpty().WithMessage("EngineTypeId is required.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
