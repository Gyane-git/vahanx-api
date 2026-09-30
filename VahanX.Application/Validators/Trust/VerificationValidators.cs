using FluentValidation;
using VahanX.Application.DTOs.Trust;

namespace VahanX.Application.Validators.Trust;

/// <summary>
/// Validator for CreateVerificationRequest.
/// </summary>
public class CreateVerificationValidator : AbstractValidator<CreateVerificationRequest>
{
    public CreateVerificationValidator()
    {
        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("VehicleId is required.");

        RuleFor(x => x.VerificationType)
            .IsInEnum().WithMessage("Invalid verification type.");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

/// <summary>
/// Validator for UpdateVerificationRequest.
/// </summary>
public class UpdateVerificationValidator : AbstractValidator<UpdateVerificationRequest>
{
    public UpdateVerificationValidator()
    {
        RuleFor(x => x.VerificationType)
            .IsInEnum().WithMessage("Invalid verification type.")
            .When(x => x.VerificationType.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

/// <summary>
/// Validator for CreateVerificationDocumentRequest.
/// </summary>
public class CreateVerificationDocumentValidator : AbstractValidator<CreateVerificationDocumentRequest>
{
    public CreateVerificationDocumentValidator()
    {
        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("DocumentType is required.")
            .MaximumLength(100).WithMessage("DocumentType cannot exceed 100 characters.");

        RuleFor(x => x.DocumentNumber)
            .MaximumLength(200).WithMessage("DocumentNumber cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.DocumentNumber));

        RuleFor(x => x.DocumentMediaReference)
            .MaximumLength(2048).WithMessage("DocumentMediaReference cannot exceed 2048 characters.")
            .When(x => !string.IsNullOrEmpty(x.DocumentMediaReference));

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
