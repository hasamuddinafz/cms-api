using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Roles.Queries.GetAllPermissions;

public record GetAllPermissionsQuery : IRequest<Result<List<PermissionGroupDto>>>;
