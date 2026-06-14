using MediatR;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.MailTemplates.Commands.CreateMailTemplate;

public class CreateMailTemplateHandler(IDbContextAccessor db)
    : IRequestHandler<CreateMailTemplateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateMailTemplateCommand request, CancellationToken ct)
    {
        var template = new MailTemplate
        {
            Name     = request.Name,
            Subject  = request.Subject,
            HtmlBody = request.HtmlBody,
        };
        db.MailTemplates.Add(template);
        await db.SaveChangesAsync(ct);
        return Result<Guid>.Success(template.Id);
    }
}
