using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.MailTemplates.Commands.UpdateMailTemplate;

public class UpdateMailTemplateHandler(IDbContextAccessor db)
    : IRequestHandler<UpdateMailTemplateCommand, Result>
{
    public async Task<Result> Handle(UpdateMailTemplateCommand request, CancellationToken ct)
    {
        var template = await db.MailTemplates.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
        if (template is null)
            return Result.Failure("Mail template not found.");

        template.Name     = request.Name;
        template.Subject  = request.Subject;
        template.HtmlBody = request.HtmlBody;
        template.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
