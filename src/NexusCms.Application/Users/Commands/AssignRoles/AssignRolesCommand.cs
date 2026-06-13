using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Users.Commands.AssignRoles;

public record AssignRolesCommand(Guid UserId, List<Guid> RoleIds) : IRequest<Result>;
