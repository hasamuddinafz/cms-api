using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;
using NexusCms.Domain.Interfaces;

namespace NexusCms.Application.Users.Commands.AssignRoles;

public class AssignRolesCommandHandler(IDbContextAccessor db, IUnitOfWork uow)
    : IRequestHandler<AssignRolesCommand, Result>
{
    public async Task<Result> Handle(AssignRolesCommand request, CancellationToken ct)
    {
        var user = await db.Users.FindAsync([request.UserId], ct);
        if (user is null) return Result.Failure("Kullanıcı bulunamadı.");

        // Remove all current role assignments
        var existing = await db.UserRoles
            .Where(ur => ur.UserId == request.UserId)
            .ToListAsync(ct);
        db.UserRoles.RemoveRange(existing);

        // Add new ones
        foreach (var roleId in request.RoleIds)
            db.UserRoles.Add(new UserRole { UserId = request.UserId, RoleId = roleId });

        await uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}
