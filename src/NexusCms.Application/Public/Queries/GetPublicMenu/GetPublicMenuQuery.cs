using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Public.Queries.GetPublicMenu;

public record GetPublicMenuQuery(string Key, string Lang = "tr") : IRequest<Result<PublicMenuDto>>;
