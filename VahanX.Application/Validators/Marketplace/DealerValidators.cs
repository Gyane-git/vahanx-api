using FluentValidation;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Application.Validators.Marketplace;

/// <summary>
/// Validator for CreateDealerRequest.
/// </summary>
public class CreateDealerValidator : AbstractValidator<CreateDealerRequest>
{
    public CreateDealerValidator()
    {
        RuleFor(x => x.BusinessName)
            .NotEmpty().WithMessage("BusinessName is required.")
            .MaximumLength(200).WithMessage("BusinessName cannot exceed 200 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Website)
            .MaximumLength(500).WithMessage("Website cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Website));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for UpdateDealerRequest.
/// </summary>
public class UpdateDealerValidator : AbstractValidator<UpdateDealerRequest>
{
    public UpdateDealerValidator()
    {
        RuleFor(x => x.BusinessName)
            .NotEmpty().WithMessage("BusinessName is required.")
            .MaximumLength(200).WithMessage("BusinessName cannot exceed 200 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Website)
            .MaximumLength(500).WithMessage("Website cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Website));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for CreateDealerBranchRequest.
/// </summary>
public class CreateDealerBranchValidator : AbstractValidator<CreateDealerBranchRequest>
{
    public CreateDealerBranchValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters.")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}

/// <summary>
/// Validator for UpdateDealerBranchRequest.
/// </summary>
public class UpdateDealerBranchValidator : AbstractValidator<UpdateDealerBranchRequest>
{
    public UpdateDealerBranchValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters.")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
