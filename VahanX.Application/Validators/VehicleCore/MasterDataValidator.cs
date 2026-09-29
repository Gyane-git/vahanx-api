using FluentValidation;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Validators.VehicleCore;

/// <summary>
/// Validator for CreateBodyTypeRequest.
/// </summary>
public class CreateBodyTypeValidator : AbstractValidator<CreateBodyTypeRequest>
{
    public CreateBodyTypeValidator()
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
/// Validator for UpdateBodyTypeRequest.
/// </summary>
public class UpdateBodyTypeValidator : AbstractValidator<UpdateBodyTypeRequest>
{
    public UpdateBodyTypeValidator()
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
/// Validator for CreateFuelTypeRequest.
/// </summary>
public class CreateFuelTypeValidator : AbstractValidator<CreateFuelTypeRequest>
{
    public CreateFuelTypeValidator()
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
/// Validator for UpdateFuelTypeRequest.
/// </summary>
public class UpdateFuelTypeValidator : AbstractValidator<UpdateFuelTypeRequest>
{
    public UpdateFuelTypeValidator()
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
/// Validator for CreateTransmissionTypeRequest.
/// </summary>
public class CreateTransmissionTypeValidator : AbstractValidator<CreateTransmissionTypeRequest>
{
    public CreateTransmissionTypeValidator()
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
/// Validator for UpdateTransmissionTypeRequest.
/// </summary>
public class UpdateTransmissionTypeValidator : AbstractValidator<UpdateTransmissionTypeRequest>
{
    public UpdateTransmissionTypeValidator()
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
/// Validator for CreateDriveTypeRequest.
/// </summary>
public class CreateDriveTypeValidator : AbstractValidator<CreateDriveTypeRequest>
{
    public CreateDriveTypeValidator()
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
/// Validator for UpdateDriveTypeRequest.
/// </summary>
public class UpdateDriveTypeValidator : AbstractValidator<UpdateDriveTypeRequest>
{
    public UpdateDriveTypeValidator()
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
/// Validator for CreateEngineTypeRequest.
/// </summary>
public class CreateEngineTypeValidator : AbstractValidator<CreateEngineTypeRequest>
{
    public CreateEngineTypeValidator()
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
/// Validator for UpdateEngineTypeRequest.
/// </summary>
public class UpdateEngineTypeValidator : AbstractValidator<UpdateEngineTypeRequest>
{
    public UpdateEngineTypeValidator()
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
/// Validator for CreateVehicleFeatureRequest.
/// </summary>
public class CreateVehicleFeatureValidator : AbstractValidator<CreateVehicleFeatureRequest>
{
    public CreateVehicleFeatureValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.FeatureGroup)
            .NotEmpty().WithMessage("FeatureGroup is required.")
            .MaximumLength(100).WithMessage("FeatureGroup cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for UpdateVehicleFeatureRequest.
/// </summary>
public class UpdateVehicleFeatureValidator : AbstractValidator<UpdateVehicleFeatureRequest>
{
    public UpdateVehicleFeatureValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.FeatureGroup)
            .NotEmpty().WithMessage("FeatureGroup is required.")
            .MaximumLength(100).WithMessage("FeatureGroup cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for CreateVehicleSpecificationRequest.
/// </summary>
public class CreateVehicleSpecificationValidator : AbstractValidator<CreateVehicleSpecificationRequest>
{
    public CreateVehicleSpecificationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Unit is required.")
            .MaximumLength(50).WithMessage("Unit cannot exceed 50 characters.");

        RuleFor(x => x.DataType)
            .NotEmpty().WithMessage("DataType is required.")
            .MaximumLength(50).WithMessage("DataType cannot exceed 50 characters.");

        RuleFor(x => x.SpecGroup)
            .MaximumLength(100).WithMessage("SpecGroup cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.SpecGroup));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for UpdateVehicleSpecificationRequest.
/// </summary>
public class UpdateVehicleSpecificationValidator : AbstractValidator<UpdateVehicleSpecificationRequest>
{
    public UpdateVehicleSpecificationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Unit is required.")
            .MaximumLength(50).WithMessage("Unit cannot exceed 50 characters.");

        RuleFor(x => x.DataType)
            .NotEmpty().WithMessage("DataType is required.")
            .MaximumLength(50).WithMessage("DataType cannot exceed 50 characters.");

        RuleFor(x => x.SpecGroup)
            .MaximumLength(100).WithMessage("SpecGroup cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.SpecGroup));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
