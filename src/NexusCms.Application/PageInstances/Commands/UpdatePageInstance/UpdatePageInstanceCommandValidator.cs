using FluentValidation;

namespace NexusCms.Application.PageInstances.Commands.UpdatePageInstance;

public class UpdatePageInstanceCommandValidator : AbstractValidator<UpdatePageInstanceCommand>
{
    public UpdatePageInstanceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(500)
            .Matches("^[a-z0-9-/]+$")
            .WithMessage("Slug yalnızca küçük harf, rakam, tire ve / içerebilir.");

        RuleFor(x => x.ContentJson)
            .NotEmpty()
            .Must(json => { try { System.Text.Json.JsonDocument.Parse(json); return true; } catch { return false; } })
            .WithMessage("ContentJson geçerli bir JSON nesnesi olmalıdır.");

        RuleFor(x => x.SeoMetaJson)
            .NotEmpty()
            .Must(json => { try { System.Text.Json.JsonDocument.Parse(json); return true; } catch { return false; } })
            .WithMessage("SeoMetaJson geçerli bir JSON nesnesi olmalıdır.");
    }
}
