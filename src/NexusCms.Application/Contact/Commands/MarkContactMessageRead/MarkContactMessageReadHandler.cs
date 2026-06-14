using NexusCms.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Domain.Common;


namespace NexusCms.Application.Contact.Commands.MarkContactMessageRead;

public class MarkContactMessageReadHandler(IDbContextAccessor db) : IRequestHandler<MarkContactMessageReadCommand, Result>
{
    public async Task<Result> Handle(MarkContactMessageReadCommand request, CancellationToken ct)
    {
        var msg = await db.ContactMessages.FindAsync([request.Id], ct);
        if (msg is null) return Result.Failure("Mesaj bulunamadı.");
        msg.IsRead = request.IsRead;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
