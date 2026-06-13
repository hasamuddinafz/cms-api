using MediatR;
using NexusCms.Application.Common.Models;

namespace NexusCms.Application.Assets.Queries.GetFileList;

public record GetFileListQuery(PagedRequest Request) : IRequest<PagedResult<FileAssetDto>>;
