using FluentValidation;

namespace NexusCms.Application.Settings.Commands.UpdateGeneralSettings;

public class UpdateGeneralSettingsCommandValidator : AbstractValidator<UpdateGeneralSettingsCommand>
{
    public UpdateGeneralSettingsCommandValidator()
    {
        RuleFor(x => x.GeneralJson)
            .NotEmpty().WithMessage("GeneralJson is required.")
            .Must(BeValidJson).WithMessage("GeneralJson must be valid JSON.");
    }

    private static bool BeValidJson(string json)
    {
        try { System.Text.Json.JsonDocument.Parse(json); return true; }
        catch { return false; }
    }
}
