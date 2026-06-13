using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;
using NexusCms.Domain.Interfaces;

namespace NexusCms.Application.Roles.Commands.UpdateRole;

public class UpdateRoleCommandHandler(IDbContextAccessor db, IUnitOfWork uow)
    : IRequestHandler<UpdateRoleCommand, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(UpdateRoleCommand request, CancellationToken ct)
    {
        var role = await db.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (role is null)
            return Result<RoleDto>.Failure("Rol bulunamadı.");

        if (role.IsSystemRole)
            return Result<RoleDto>.Failure("Sistem rolleri düzenlenemez.");

        if (await db.Roles.AnyAsync(r => r.Name == request.Name && r.Id != request.Id, ct))
            return Result<RoleDto>.Failure($"'{request.Name}' adında başka bir rol zaten mevcut.");

        role.Name        = request.Name;
        role.Description = request.Description;

        // Replace permissions
        db.RolePermissions.RemoveRange(role.RolePermissions);
        role.RolePermissions = request.PermissionIds
            .Select(pid => new RolePermission { RoleId = role.Id, PermissionId = pid })
            .ToList();

        await uow.SaveChangesAsync(ct);

        return Result<RoleDto>.Success(new RoleDto(
            role.Id, role.Name!, role.Description, role.IsSystemRole,
            request.PermissionIds.Select(p => p.ToString()).ToList(),
            role.CreatedAt));
    }
}
