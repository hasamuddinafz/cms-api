using FluentValidation;

namespace NexusCms.Application.Mail.Commands.SendMail;

public class SendMailCommandValidator : AbstractValidator<SendMailCommand>
{
    public SendMailCommandValidator()
    {
        RuleFor(x => x.ToEmail).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.ToName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(500);
        RuleFor(x => x.HtmlBody).NotEmpty();
    }
}
