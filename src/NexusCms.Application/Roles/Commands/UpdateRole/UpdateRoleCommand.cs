using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(
    Guid       Id,
    string     Name,
    string?    Description,
    List<Guid> PermissionIds) : IRequest<Result<RoleDto>>;
