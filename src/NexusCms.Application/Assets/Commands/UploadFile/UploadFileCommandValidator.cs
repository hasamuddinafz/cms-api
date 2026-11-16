using FluentValidation;
using Microsoft.Extensions.Options;
using NexusCms.Application.Common.Options;

namespace NexusCms.Application.Assets.Commands.UploadFile;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator(IOptions<FileStorageOptions> options)
    {
        var opts = options.Value;

        RuleFor(x => x.OriginalFileName).NotEmpty().MaximumLength(500);

        RuleFor(x => x.MimeType)
            .NotEmpty()
            .Must(mime => opts.AllowedMimeTypes.Count == 0 || opts.AllowedMimeTypes.Contains(mime))
            .WithMessage(x => $"MIME type '{x.MimeType}' is not supported.");

        RuleFor(x => x.SizeBytes)
            .GreaterThan(0)
            .WithMessage("File cannot be empty.");

        RuleFor(x => x.Folder).MaximumLength(200).When(x => x.Folder is not null);
    }
}
