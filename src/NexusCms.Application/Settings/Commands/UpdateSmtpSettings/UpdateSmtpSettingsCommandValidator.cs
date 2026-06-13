using FluentValidation;

namespace NexusCms.Application.Settings.Commands.UpdateSmtpSettings;

public class UpdateSmtpSettingsCommandValidator : AbstractValidator<UpdateSmtpSettingsCommand>
{
    public UpdateSmtpSettingsCommandValidator()
    {
        RuleFor(x => x.SmtpJson)
            .NotEmpty().WithMessage("SmtpJson is required.")
            .Must(BeValidJson).WithMessage("SmtpJson must be valid JSON.");
    }

    private static bool BeValidJson(string json)
    {
        try { System.Text.Json.JsonDocument.Parse(json); return true; }
        catch { return false; }
    }
}
