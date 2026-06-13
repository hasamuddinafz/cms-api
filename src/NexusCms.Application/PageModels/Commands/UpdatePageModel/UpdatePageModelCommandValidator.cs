using FluentValidation;

namespace NexusCms.Application.PageModels.Commands.UpdatePageModel;

public class UpdatePageModelCommandValidator : AbstractValidator<UpdatePageModelCommand>
{
    public UpdatePageModelCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Icon).MaximumLength(100);
        RuleFor(x => x.SchemaJson)
            .NotEmpty()
            .Must(json =>
            {
                try { System.Text.Json.JsonDocument.Parse(json); return true; }
                catch { return false; }
            }).WithMessage("SchemaJson geçerli bir JSON nesnesi olmalıdır.");
    }
}
