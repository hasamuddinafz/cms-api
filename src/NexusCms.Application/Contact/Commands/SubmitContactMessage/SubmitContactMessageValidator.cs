using FluentValidation;

namespace NexusCms.Application.Contact.Commands.SubmitContactMessage;

public class SubmitContactMessageValidator : AbstractValidator<SubmitContactMessageCommand>
{
    public SubmitContactMessageValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Phone).MaximumLength(50);
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(5000);
    }
}
