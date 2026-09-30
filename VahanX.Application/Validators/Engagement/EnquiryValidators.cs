using FluentValidation;
using VahanX.Application.DTOs.Engagement;

namespace VahanX.Application.Validators.Engagement;

/// <summary>
/// Validator for CreateEnquiryRequest.
/// </summary>
public class CreateEnquiryValidator : AbstractValidator<CreateEnquiryRequest>
{
    public CreateEnquiryValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("ListingId is required.");

        RuleFor(x => x.EnquiryType)
            .IsInEnum().WithMessage("Invalid enquiry type.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required.")
            .MaximumLength(300).WithMessage("Subject cannot exceed 300 characters.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(2000).WithMessage("Message cannot exceed 2000 characters.");
    }
}

/// <summary>
/// Validator for UpdateEnquiryRequest.
/// </summary>
public class UpdateEnquiryValidator : AbstractValidator<UpdateEnquiryRequest>
{
    public UpdateEnquiryValidator()
    {
        RuleFor(x => x.Subject)
            .MaximumLength(300).WithMessage("Subject cannot exceed 300 characters.")
            .When(x => !string.IsNullOrEmpty(x.Subject));

        RuleFor(x => x.Message)
            .MaximumLength(2000).WithMessage("Message cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Message));
    }
}

/// <summary>
/// Validator for RespondEnquiryRequest.
/// </summary>
public class RespondEnquiryValidator : AbstractValidator<RespondEnquiryRequest>
{
    public RespondEnquiryValidator()
    {
        RuleFor(x => x.Response)
            .NotEmpty().WithMessage("Response is required.")
            .MaximumLength(2000).WithMessage("Response cannot exceed 2000 characters.");
    }
}
