using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.MailTemplates.Commands.DeleteMailTemplate;

public class DeleteMailTemplateHandler(IDbContextAccessor db)
    : IRequestHandler<DeleteMailTemplateCommand, Result>
{
    public async Task<Result> Handle(DeleteMailTemplateCommand request, CancellationToken ct)
    {
        var template = await db.MailTemplates.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
        if (template is null)
            return Result.Failure("Mail şablonu bulunamadı.");

        db.MailTemplates.Remove(template);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
