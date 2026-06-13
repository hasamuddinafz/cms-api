using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Roles.Commands.CreateRole;

public record CreateRoleCommand(
    string       Name,
    string?      Description,
    List<Guid>   PermissionIds) : IRequest<Result<RoleDto>>;
