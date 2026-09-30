using FluentValidation;
using VahanX.Application.DTOs.Business;

namespace VahanX.Application.Validators.Business;

/// <summary>
/// Validator for CreateFeatureRequest.
/// </summary>
public class CreateFeatureValidator : AbstractValidator<CreateFeatureRequest>
{
    public CreateFeatureValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(100).WithMessage("Code cannot exceed 100 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.FeatureType)
            .IsInEnum().WithMessage("Invalid feature type.");
    }
}

/// <summary>
/// Validator for CreateSubscriptionPlanRequest.
/// </summary>
public class CreateSubscriptionPlanValidator : AbstractValidator<CreateSubscriptionPlanRequest>
{
    public CreateSubscriptionPlanValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative.");

        RuleFor(x => x.TrialDays)
            .GreaterThanOrEqualTo(0).WithMessage("TrialDays must be non-negative.");

        RuleFor(x => x.TargetType)
            .IsInEnum().WithMessage("Invalid target type.");

        RuleFor(x => x.BillingCycle)
            .IsInEnum().WithMessage("Invalid billing cycle.");
    }
}

/// <summary>
/// Validator for CreateSubscriptionRequest.
/// </summary>
public class CreateSubscriptionValidator : AbstractValidator<CreateSubscriptionRequest>
{
    public CreateSubscriptionValidator()
    {
        RuleFor(x => x.SubscriptionPlanId)
            .NotEmpty().WithMessage("SubscriptionPlanId is required.");
    }
}

/// <summary>
/// Validator for ChangePlanRequest.
/// </summary>
public class ChangePlanValidator : AbstractValidator<ChangePlanRequest>
{
    public ChangePlanValidator()
    {
        RuleFor(x => x.NewPlanId)
            .NotEmpty().WithMessage("NewPlanId is required.");
    }
}
