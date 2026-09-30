using FluentValidation;
using VahanX.Application.DTOs.Engagement;

namespace VahanX.Application.Validators.Engagement;

/// <summary>
/// Validator for CreateConversationRequest.
/// </summary>
public class CreateConversationValidator : AbstractValidator<CreateConversationRequest>
{
    public CreateConversationValidator()
    {
        RuleFor(x => x.ConversationType)
            .IsInEnum().WithMessage("Invalid conversation type.");

        RuleFor(x => x.ParticipantUserIds)
            .NotEmpty().WithMessage("At least one participant is required.");

        RuleFor(x => x.ParticipantUserIds)
            .Must(x => x.Count <= 10).WithMessage("Maximum 10 participants allowed.");
    }
}

/// <summary>
/// Validator for SendMessageRequest.
/// </summary>
public class SendMessageValidator : AbstractValidator<SendMessageRequest>
{
    public SendMessageValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(4000).WithMessage("Content cannot exceed 4000 characters.");

        RuleFor(x => x.MessageType)
            .IsInEnum().WithMessage("Invalid message type.");
    }
}

/// <summary>
/// Validator for UpdateMessageRequest.
/// </summary>
public class UpdateMessageValidator : AbstractValidator<UpdateMessageRequest>
{
    public UpdateMessageValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(4000).WithMessage("Content cannot exceed 4000 characters.");
    }
}
