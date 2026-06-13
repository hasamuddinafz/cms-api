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
        if (role is null) return Result.Failure("Rol bulunamadı.");
        if (role.IsSystemRole) return Result.Failure("Sistem rolleri silinemez.");

        db.Roles.Remove(role);
        await uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}
