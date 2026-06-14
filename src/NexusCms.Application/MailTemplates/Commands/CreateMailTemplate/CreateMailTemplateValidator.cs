using FluentValidation;

namespace NexusCms.Application.MailTemplates.Commands.CreateMailTemplate;

public class CreateMailTemplateValidator : AbstractValidator<CreateMailTemplateCommand>
{
    public CreateMailTemplateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(500);
        RuleFor(x => x.HtmlBody).NotEmpty();
    }
}
