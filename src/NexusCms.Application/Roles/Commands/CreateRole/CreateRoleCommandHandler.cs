using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;
using NexusCms.Domain.Interfaces;

namespace NexusCms.Application.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler(IDbContextAccessor db, IUnitOfWork uow)
    : IRequestHandler<CreateRoleCommand, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(CreateRoleCommand request, CancellationToken ct)
    {
        if (await db.Roles.AnyAsync(r => r.Name == request.Name, ct))
            return Result<RoleDto>.Failure($"A role named '{request.Name}' already exists.");

        var role = new Role
        {
            Name         = request.Name,
            Description  = request.Description,
            IsSystemRole = false,
        };

        role.RolePermissions = request.PermissionIds
            .Select(pid => new RolePermission { RoleId = role.Id, PermissionId = pid })
            .ToList();

        db.Roles.Add(role);
        await uow.SaveChangesAsync(ct);

        return Result<RoleDto>.Success(new RoleDto(
            role.Id, role.Name!, role.Description, false,
            role.RolePermissions.Select(rp => rp.PermissionId.ToString()).ToList(),
            role.CreatedAt));
    }
}
