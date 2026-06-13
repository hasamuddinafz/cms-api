using MediatR;
using NexusCms.Application.Common.Models;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Users.Queries.GetUserList;

public record GetUserListQuery(string Search = "", int Page = 1, int PageSize = 20)
    : IRequest<Result<PagedResult<UserListItemDto>>>;
