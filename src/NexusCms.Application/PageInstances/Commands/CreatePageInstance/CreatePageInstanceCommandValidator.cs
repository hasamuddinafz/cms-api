using FluentValidation;

namespace NexusCms.Application.PageInstances.Commands.CreatePageInstance;

public class CreatePageInstanceCommandValidator : AbstractValidator<CreatePageInstanceCommand>
{
    public CreatePageInstanceCommandValidator()
    {
        RuleFor(x => x.PageModelId).NotEmpty();

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(500)
            .Matches("^[a-z0-9-/]+$")
            .WithMessage("Slug yalnızca küçük harf, rakam, tire ve / içerebilir.");

        RuleFor(x => x.ContentJson)
            .NotEmpty()
            .Must(BeValidJson).WithMessage("ContentJson geçerli bir JSON nesnesi olmalıdır.");

        RuleFor(x => x.SeoMetaJson)
            .NotEmpty()
            .Must(BeValidJson).WithMessage("SeoMetaJson geçerli bir JSON nesnesi olmalıdır.");
    }

    private static bool BeValidJson(string json)
    {
        try { System.Text.Json.JsonDocument.Parse(json); return true; }
        catch { return false; }
    }
}
