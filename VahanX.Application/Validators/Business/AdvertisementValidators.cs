using FluentValidation;
using VahanX.Application.DTOs.Business;

namespace VahanX.Application.Validators.Business;

/// <summary>
/// Validator for CreateAdvertisementCampaignRequest.
/// </summary>
public class CreateAdvertisementCampaignValidator : AbstractValidator<CreateAdvertisementCampaignRequest>
{
    public CreateAdvertisementCampaignValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("EndDate is required.")
            .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate.");

        RuleFor(x => x.BudgetAmount)
            .GreaterThan(0).WithMessage("BudgetAmount must be positive.");

        RuleFor(x => x.DailyBudget)
            .GreaterThan(0).WithMessage("DailyBudget must be positive.")
            .When(x => x.DailyBudget.HasValue);

        RuleFor(x => x.TotalBudget)
            .GreaterThan(0).WithMessage("TotalBudget must be positive.")
            .When(x => x.TotalBudget.HasValue);
    }
}

/// <summary>
/// Validator for CreateAdvertisementRequest.
/// </summary>
public class CreateAdvertisementValidator : AbstractValidator<CreateAdvertisementRequest>
{
    public CreateAdvertisementValidator()
    {
        RuleFor(x => x.CampaignId)
            .NotEmpty().WithMessage("CampaignId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Headline)
            .MaximumLength(300).WithMessage("Headline cannot exceed 300 characters.")
            .When(x => !string.IsNullOrEmpty(x.Headline));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.DestinationType)
            .IsInEnum().WithMessage("Invalid destination type.");

        RuleFor(x => x.DestinationUrl)
            .MaximumLength(2048).WithMessage("DestinationUrl cannot exceed 2048 characters.")
            .When(x => !string.IsNullOrEmpty(x.DestinationUrl));
    }
}
