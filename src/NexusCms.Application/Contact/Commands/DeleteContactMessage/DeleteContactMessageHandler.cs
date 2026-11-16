using NexusCms.Application.Common.Interfaces;
using MediatR;
using NexusCms.Domain.Common;


namespace NexusCms.Application.Contact.Commands.DeleteContactMessage;

public class DeleteContactMessageHandler(IDbContextAccessor db) : IRequestHandler<DeleteContactMessageCommand, Result>
{
    public async Task<Result> Handle(DeleteContactMessageCommand request, CancellationToken ct)
    {
        var msg = await db.ContactMessages.FindAsync([request.Id], ct);
        if (msg is null) return Result.Failure("Message not found.");
        db.ContactMessages.Remove(msg);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
