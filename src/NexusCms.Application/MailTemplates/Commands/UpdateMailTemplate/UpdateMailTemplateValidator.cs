using FluentValidation;

namespace NexusCms.Application.MailTemplates.Commands.UpdateMailTemplate;

public class UpdateMailTemplateValidator : AbstractValidator<UpdateMailTemplateCommand>
{
    public UpdateMailTemplateValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(500);
        RuleFor(x => x.HtmlBody).NotEmpty();
    }
}
