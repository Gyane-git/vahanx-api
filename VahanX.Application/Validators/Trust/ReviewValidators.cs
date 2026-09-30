using FluentValidation;
using VahanX.Application.DTOs.Trust;

namespace VahanX.Application.Validators.Trust;

/// <summary>
/// Validator for CreateReviewRequest.
/// </summary>
public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Comment)
            .MaximumLength(2000).WithMessage("Comment cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Comment));
    }
}

/// <summary>
/// Validator for UpdateReviewRequest.
/// </summary>
public class UpdateReviewValidator : AbstractValidator<UpdateReviewRequest>
{
    public UpdateReviewValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.")
            .When(x => x.Rating.HasValue);

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Comment)
            .MaximumLength(2000).WithMessage("Comment cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Comment));
    }
}

/// <summary>
/// Validator for ReportReviewRequest.
/// </summary>
public class ReportReviewValidator : AbstractValidator<ReportReviewRequest>
{
    public ReportReviewValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(200).WithMessage("Reason cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
