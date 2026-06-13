using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Assets.Queries.GetFileById;

public record GetFileByIdQuery(Guid Id) : IRequest<Result<FileAssetDto>>;
