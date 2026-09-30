using FluentValidation;
using VahanX.Application.DTOs.Services;

namespace VahanX.Application.Validators.Services;

/// <summary>
/// Validator for CreateServiceBookingRequest.
/// </summary>
public class CreateServiceBookingValidator : AbstractValidator<CreateServiceBookingRequest>
{
    public CreateServiceBookingValidator()
    {
        RuleFor(x => x.ServiceCenterBranchId)
            .NotEmpty().WithMessage("ServiceCenterBranchId is required.");

        RuleFor(x => x.RequestedDate)
            .NotEmpty().WithMessage("RequestedDate is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("RequestedDate cannot be in the past.");

        RuleFor(x => x.StartTime)
            .GreaterThan(x => x.RequestedDate).WithMessage("StartTime must be after RequestedDate.")
            .When(x => x.StartTime.HasValue);

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("EndTime must be after StartTime.")
            .When(x => x.EndTime.HasValue && x.StartTime.HasValue);

        RuleFor(x => x.CustomerNotes)
            .MaximumLength(2000).WithMessage("CustomerNotes cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.CustomerNotes));
    }
}

/// <summary>
/// Validator for UpdateServiceBookingRequest.
/// </summary>
public class UpdateServiceBookingValidator : AbstractValidator<UpdateServiceBookingRequest>
{
    public UpdateServiceBookingValidator()
    {
        RuleFor(x => x.StartTime)
            .GreaterThan(x => x.RequestedDate).WithMessage("StartTime must be after RequestedDate.")
            .When(x => x.StartTime.HasValue && x.RequestedDate.HasValue);

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("EndTime must be after StartTime.")
            .When(x => x.EndTime.HasValue && x.StartTime.HasValue);

        RuleFor(x => x.CustomerNotes)
            .MaximumLength(2000).WithMessage("CustomerNotes cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.CustomerNotes));
    }
}

/// <summary>
/// Validator for RescheduleServiceBookingRequest.
/// </summary>
public class RescheduleServiceBookingValidator : AbstractValidator<RescheduleServiceBookingRequest>
{
    public RescheduleServiceBookingValidator()
    {
        RuleFor(x => x.NewStartTime)
            .NotEmpty().WithMessage("NewStartTime is required.");

        RuleFor(x => x.NewEndTime)
            .NotEmpty().WithMessage("NewEndTime is required.")
            .GreaterThan(x => x.NewStartTime).WithMessage("NewEndTime must be after NewStartTime.");
    }
}
