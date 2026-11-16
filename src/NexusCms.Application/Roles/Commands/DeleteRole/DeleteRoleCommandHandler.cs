using MediatR;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Interfaces;

namespace NexusCms.Application.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler(IDbContextAccessor db, IUnitOfWork uow)
    : IRequestHandler<DeleteRoleCommand, Result>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken ct)
    {
        var role = await db.Roles.FindAsync([request.Id], ct);
        if (role is null) return Result.Failure("Role not found.");
        if (role.IsSystemRole) return Result.Failure("System roles cannot be deleted.");

        db.Roles.Remove(role);
        await uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}
