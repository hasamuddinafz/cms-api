using MediatR;

namespace NexusCms.Application.Menus.Queries.GetMenuList;

public record GetMenuListQuery : IRequest<IReadOnlyList<MenuDto>>;
