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
            .WithMessage("Slug may only contain lowercase letters, digits, hyphens and /.");

        RuleFor(x => x.ContentJson)
            .NotEmpty()
            .Must(BeValidJson).WithMessage("ContentJson must be a valid JSON object.");

        RuleFor(x => x.SeoMetaJson)
            .NotEmpty()
            .Must(BeValidJson).WithMessage("SeoMetaJson must be a valid JSON object.");
    }

    private static bool BeValidJson(string json)
    {
        try { System.Text.Json.JsonDocument.Parse(json); return true; }
        catch { return false; }
    }
}
