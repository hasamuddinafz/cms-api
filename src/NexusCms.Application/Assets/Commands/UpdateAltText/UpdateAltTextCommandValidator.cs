using FluentValidation;

namespace NexusCms.Application.Assets.Commands.UpdateAltText;

public class UpdateAltTextCommandValidator : AbstractValidator<UpdateAltTextCommand>
{
    public UpdateAltTextCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AltTextJson)
            .NotEmpty()
            .Must(json => { try { System.Text.Json.JsonDocument.Parse(json); return true; } catch { return false; } })
            .WithMessage("AltTextJson must be a valid JSON object.");
    }
}
