using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Roles.Queries.GetAllPermissions;

public class GetAllPermissionsQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetAllPermissionsQuery, Result<List<PermissionGroupDto>>>
{
    public async Task<Result<List<PermissionGroupDto>>> Handle(GetAllPermissionsQuery request, CancellationToken ct)
    {
        var permissions = await db.Permissions
            .AsNoTracking()
            .OrderBy(p => p.Module).ThenBy(p => p.Key)
            .ToListAsync(ct);

        var groups = permissions
            .GroupBy(p => p.Module)
            .Select(g => new PermissionGroupDto(
                g.Key,
                g.Select(p => new PermissionItemDto(p.Id, p.Key, p.Label)).ToList()))
            .ToList();

        return Result<List<PermissionGroupDto>>.Success(groups);
    }
}
