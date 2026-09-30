using FluentValidation;
using VahanX.Application.DTOs.Engagement;

namespace VahanX.Application.Validators.Engagement;

/// <summary>
/// Validator for CreateTestDriveRequest.
/// </summary>
public class CreateTestDriveValidator : AbstractValidator<CreateTestDriveRequest>
{
    public CreateTestDriveValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("ListingId is required.");

        RuleFor(x => x.RequestedDate)
            .NotEmpty().WithMessage("RequestedDate is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("RequestedDate cannot be in the past.");

        RuleFor(x => x.StartTime)
            .GreaterThan(x => x.RequestedDate).WithMessage("StartTime must be after RequestedDate.")
            .When(x => x.StartTime.HasValue);

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("EndTime must be after StartTime.")
            .When(x => x.EndTime.HasValue && x.StartTime.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

/// <summary>
/// Validator for UpdateTestDriveRequest.
/// </summary>
public class UpdateTestDriveValidator : AbstractValidator<UpdateTestDriveRequest>
{
    public UpdateTestDriveValidator()
    {
        RuleFor(x => x.StartTime)
            .GreaterThan(x => x.RequestedDate).WithMessage("StartTime must be after RequestedDate.")
            .When(x => x.StartTime.HasValue && x.RequestedDate.HasValue);

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("EndTime must be after StartTime.")
            .When(x => x.EndTime.HasValue && x.StartTime.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

/// <summary>
/// Validator for RescheduleTestDriveRequest.
/// </summary>
public class RescheduleTestDriveValidator : AbstractValidator<RescheduleTestDriveRequest>
{
    public RescheduleTestDriveValidator()
    {
        RuleFor(x => x.NewStartTime)
            .NotEmpty().WithMessage("NewStartTime is required.");

        RuleFor(x => x.NewEndTime)
            .NotEmpty().WithMessage("NewEndTime is required.")
            .GreaterThan(x => x.NewStartTime).WithMessage("NewEndTime must be after NewStartTime.");
    }
}
