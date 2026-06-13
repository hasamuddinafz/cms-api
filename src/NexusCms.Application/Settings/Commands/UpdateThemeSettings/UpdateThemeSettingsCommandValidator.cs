using FluentValidation;

namespace NexusCms.Application.Settings.Commands.UpdateThemeSettings;

public class UpdateThemeSettingsCommandValidator : AbstractValidator<UpdateThemeSettingsCommand>
{
    public UpdateThemeSettingsCommandValidator()
    {
        RuleFor(x => x.ThemeJson)
            .NotEmpty().WithMessage("ThemeJson is required.")
            .Must(BeValidJson).WithMessage("ThemeJson must be valid JSON.");
    }

    private static bool BeValidJson(string json)
    {
        try { System.Text.Json.JsonDocument.Parse(json); return true; }
        catch { return false; }
    }
}
