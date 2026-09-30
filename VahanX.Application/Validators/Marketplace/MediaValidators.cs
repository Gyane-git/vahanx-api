using FluentValidation;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Application.Validators.Marketplace;

/// <summary>
/// Validator for CreateListingMediaRequest.
/// </summary>
public class CreateListingMediaValidator : AbstractValidator<CreateListingMediaRequest>
{
    public CreateListingMediaValidator()
    {
        RuleFor(x => x.MediaUrl)
            .NotEmpty().WithMessage("MediaUrl is required.")
            .MaximumLength(2048).WithMessage("MediaUrl cannot exceed 2048 characters.");

        RuleFor(x => x.Caption)
            .MaximumLength(500).WithMessage("Caption cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Caption));
    }
}

/// <summary>
/// Validator for UpdateListingMediaRequest.
/// </summary>
public class UpdateListingMediaValidator : AbstractValidator<UpdateListingMediaRequest>
{
    public UpdateListingMediaValidator()
    {
        RuleFor(x => x.MediaUrl)
            .NotEmpty().WithMessage("MediaUrl is required.")
            .MaximumLength(2048).WithMessage("MediaUrl cannot exceed 2048 characters.")
            .When(x => !string.IsNullOrEmpty(x.MediaUrl));

        RuleFor(x => x.Caption)
            .MaximumLength(500).WithMessage("Caption cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Caption));
    }
}
