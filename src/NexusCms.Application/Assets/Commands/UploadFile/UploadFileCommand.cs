using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Assets.Commands.UploadFile;

public record UploadFileCommand(
    Stream Content,
    string OriginalFileName,
    string MimeType,
    long SizeBytes,
    string? Folder
) : IRequest<Result<FileAssetDto>>;
