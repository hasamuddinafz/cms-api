using FluentValidation;

namespace NexusCms.Application.Languages.Commands.DeleteLanguage;

public class DeleteLanguageCommandValidator : AbstractValidator<DeleteLanguageCommand>
{
    public DeleteLanguageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
