using FluentValidation;
using VahanX.Application.DTOs.Services;

namespace VahanX.Application.Validators.Services;

/// <summary>
/// Validator for CreateServiceCenterRequest.
/// </summary>
public class CreateServiceCenterValidator : AbstractValidator<CreateServiceCenterRequest>
{
    public CreateServiceCenterValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.ContactPhone)
            .NotEmpty().WithMessage("ContactPhone is required.")
            .MaximumLength(20).WithMessage("ContactPhone cannot exceed 20 characters.");

        RuleFor(x => x.ContactEmail)
            .EmailAddress().WithMessage("ContactEmail must be a valid email address.")
            .When(x => !string.IsNullOrEmpty(x.ContactEmail));

        RuleFor(x => x.WebsiteUrl)
            .MaximumLength(500).WithMessage("WebsiteUrl cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.WebsiteUrl));
    }
}

/// <summary>
/// Validator for UpdateServiceCenterRequest.
/// </summary>
public class UpdateServiceCenterValidator : AbstractValidator<UpdateServiceCenterRequest>
{
    public UpdateServiceCenterValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.ContactPhone)
            .MaximumLength(20).WithMessage("ContactPhone cannot exceed 20 characters.")
            .When(x => !string.IsNullOrEmpty(x.ContactPhone));

        RuleFor(x => x.ContactEmail)
            .EmailAddress().WithMessage("ContactEmail must be a valid email address.")
            .When(x => !string.IsNullOrEmpty(x.ContactEmail));
    }
}

/// <summary>
/// Validator for CreateServiceCenterBranchRequest.
/// </summary>
public class CreateServiceCenterBranchValidator : AbstractValidator<CreateServiceCenterBranchRequest>
{
    public CreateServiceCenterBranchValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.")
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.")
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.ContactPhone)
            .MaximumLength(20).WithMessage("ContactPhone cannot exceed 20 characters.")
            .When(x => !string.IsNullOrEmpty(x.ContactPhone));

        RuleFor(x => x.ContactEmail)
            .EmailAddress().WithMessage("ContactEmail must be a valid email address.")
            .When(x => !string.IsNullOrEmpty(x.ContactEmail));
    }
}

/// <summary>
/// Validator for UpdateServiceCenterBranchRequest.
/// </summary>
public class UpdateServiceCenterBranchValidator : AbstractValidator<UpdateServiceCenterBranchRequest>
{
    public UpdateServiceCenterBranchValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.")
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.")
            .When(x => x.Longitude.HasValue);
    }
}
