using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IRequest<Result<RoleDto>>;
