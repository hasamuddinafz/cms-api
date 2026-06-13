using FluentValidation;

namespace NexusCms.Application.Settings.Commands.UpdateSeoSettings;

public class UpdateSeoSettingsCommandValidator : AbstractValidator<UpdateSeoSettingsCommand>
{
    public UpdateSeoSettingsCommandValidator()
    {
        RuleFor(x => x.SeoJson)
            .NotEmpty().WithMessage("SeoJson is required.")
            .Must(BeValidJson).WithMessage("SeoJson must be valid JSON.");
    }

    private static bool BeValidJson(string json)
    {
        try { System.Text.Json.JsonDocument.Parse(json); return true; }
        catch { return false; }
    }
}
