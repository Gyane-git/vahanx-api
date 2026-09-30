using FluentValidation;
using VahanX.Application.DTOs.Engagement;

namespace VahanX.Application.Validators.Engagement;

/// <summary>
/// Validator for UpdateNotificationPreferenceRequest.
/// </summary>
public class UpdateNotificationPreferenceValidator : AbstractValidator<UpdateNotificationPreferenceRequest>
{
    public UpdateNotificationPreferenceValidator()
    {
        RuleFor(x => x.NotificationType)
            .IsInEnum().WithMessage("Invalid notification type.");
    }
}

/// <summary>
/// Validator for RegisterPushTokenRequest.
/// </summary>
public class RegisterPushTokenValidator : AbstractValidator<RegisterPushTokenRequest>
{
    public RegisterPushTokenValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required.")
            .MaximumLength(500).WithMessage("Token cannot exceed 500 characters.");

        RuleFor(x => x.Platform)
            .IsInEnum().WithMessage("Invalid platform.");

        RuleFor(x => x.DeviceId)
            .MaximumLength(200).WithMessage("DeviceId cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.DeviceId));
    }
}
