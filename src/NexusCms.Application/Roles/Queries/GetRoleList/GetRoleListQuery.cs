using MediatR;
using NexusCms.Application.Common.Models;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Roles.Queries.GetRoleList;

public record GetRoleListQuery(string Search = "", int Page = 1, int PageSize = 20)
    : IRequest<Result<PagedResult<RoleListItemDto>>>;
