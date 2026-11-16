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
            .WithMessage("Key may only contain lowercase letters, digits and hyphens.");

        RuleFor(x => x.ItemsJson)
            .NotEmpty()
            .Must(json =>
            {
                try { System.Text.Json.JsonDocument.Parse(json); return true; }
                catch { return false; }
            })
            .WithMessage("ItemsJson must be a valid JSON array.");
    }
}
