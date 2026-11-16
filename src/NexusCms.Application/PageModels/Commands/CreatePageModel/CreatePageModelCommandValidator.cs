using FluentValidation;

namespace NexusCms.Application.PageModels.Commands.CreatePageModel;

public class CreatePageModelCommandValidator : AbstractValidator<CreatePageModelCommand>
{
    public CreatePageModelCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .MaximumLength(100)
            .Matches("^[a-z0-9-]+$").WithMessage("Key may only contain lowercase letters, digits and hyphens.");

        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Icon).MaximumLength(100);

        RuleFor(x => x.SchemaJson)
            .NotEmpty()
            .Must(BeValidJson).WithMessage("SchemaJson must be a valid JSON object.");
    }

    private static bool BeValidJson(string json)
    {
        try
        {
            System.Text.Json.JsonDocument.Parse(json);
            return true;
        }
        catch { return false; }
    }
}
