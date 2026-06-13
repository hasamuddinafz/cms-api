using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Menus.Queries.GetMenuByKey;

public record GetMenuByKeyQuery(string Key) : IRequest<Result<MenuDto>>;
