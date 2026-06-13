using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(Guid Id) : IRequest<Result>;
