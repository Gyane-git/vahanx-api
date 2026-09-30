using FluentValidation;
using VahanX.Application.DTOs.Trust;

namespace VahanX.Application.Validators.Trust;

/// <summary>
/// Validator for CreateInspectionRequest.
/// </summary>
public class CreateInspectionValidator : AbstractValidator<CreateInspectionRequest>
{
    public CreateInspectionValidator()
    {
        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("VehicleId is required.");

        RuleFor(x => x.InspectionType)
            .IsInEnum().WithMessage("Invalid inspection type.");
    }
}

/// <summary>
/// Validator for UpdateInspectionRequest.
/// </summary>
public class UpdateInspectionValidator : AbstractValidator<UpdateInspectionRequest>
{
    public UpdateInspectionValidator()
    {
        RuleFor(x => x.InspectionType)
            .IsInEnum().WithMessage("Invalid inspection type.")
            .When(x => x.InspectionType.HasValue);

        RuleFor(x => x.Summary)
            .MaximumLength(2000).WithMessage("Summary cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Summary));

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

/// <summary>
/// Validator for CreateInspectionItemRequest.
/// </summary>
public class CreateInspectionItemValidator : AbstractValidator<CreateInspectionItemRequest>
{
    public CreateInspectionItemValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(100).WithMessage("Category cannot exceed 100 characters.");

        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("ItemName is required.")
            .MaximumLength(200).WithMessage("ItemName cannot exceed 200 characters.");

        RuleFor(x => x.Condition)
            .IsInEnum().WithMessage("Invalid condition.");

        RuleFor(x => x.Score)
            .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.")
            .When(x => x.Score.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

/// <summary>
/// Validator for UpdateInspectionItemRequest.
/// </summary>
public class UpdateInspectionItemValidator : AbstractValidator<UpdateInspectionItemRequest>
{
    public UpdateInspectionItemValidator()
    {
        RuleFor(x => x.Condition)
            .IsInEnum().WithMessage("Invalid condition.")
            .When(x => x.Condition.HasValue);

        RuleFor(x => x.Score)
            .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.")
            .When(x => x.Score.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
