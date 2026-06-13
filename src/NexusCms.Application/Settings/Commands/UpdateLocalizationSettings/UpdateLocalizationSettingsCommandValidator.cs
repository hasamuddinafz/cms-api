using FluentValidation;

namespace NexusCms.Application.Settings.Commands.UpdateLocalizationSettings;

public class UpdateLocalizationSettingsCommandValidator : AbstractValidator<UpdateLocalizationSettingsCommand>
{
    public UpdateLocalizationSettingsCommandValidator()
    {
        RuleFor(x => x.LocalizationJson)
            .NotEmpty().WithMessage("LocalizationJson is required.")
            .Must(BeValidJson).WithMessage("LocalizationJson must be valid JSON.");
    }

    private static bool BeValidJson(string json)
    {
        try { System.Text.Json.JsonDocument.Parse(json); return true; }
        catch { return false; }
    }
}
