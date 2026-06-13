using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetRoleByIdQuery, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(GetRoleByIdQuery request, CancellationToken ct)
    {
        var role = await db.Roles
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (role is null)
            return Result<RoleDto>.Failure("Rol bulunamadı.");

        return Result<RoleDto>.Success(new RoleDto(
            role.Id,
            role.Name!,
            role.Description,
            role.IsSystemRole,
            role.RolePermissions.Select(rp => rp.Permission.Key).ToList(),
            role.CreatedAt));
    }
}
