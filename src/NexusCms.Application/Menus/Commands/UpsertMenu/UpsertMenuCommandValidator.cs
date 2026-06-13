using FluentValidation;

namespace NexusCms.Application.Menus.Commands.UpsertMenu;

public class UpsertMenuCommandValidator : AbstractValidator<UpsertMenuCommand>
{
    public UpsertMenuCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .MaximumLength(100)
            .Matches("^[a-z0-9-]+$")
            .WithMessage("Key yalnızca küçük harf, rakam ve tire içerebilir.");

        RuleFor(x => x.ItemsJson)
            .NotEmpty()
            .Must(json =>
            {
                try { System.Text.Json.JsonDocument.Parse(json); return true; }
                catch { return false; }
            })
            .WithMessage("ItemsJson geçerli bir JSON dizisi olmalıdır.");
    }
}
