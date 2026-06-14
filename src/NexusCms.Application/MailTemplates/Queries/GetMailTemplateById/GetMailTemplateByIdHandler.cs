using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.MailTemplates;
using NexusCms.Domain.Common;

namespace NexusCms.Application.MailTemplates.Queries.GetMailTemplateById;

public class GetMailTemplateByIdHandler(IDbContextAccessor db)
    : IRequestHandler<GetMailTemplateByIdQuery, Result<MailTemplateDto>>
{
    public async Task<Result<MailTemplateDto>> Handle(GetMailTemplateByIdQuery request, CancellationToken ct)
    {
        var template = await db.MailTemplates
            .FirstOrDefaultAsync(t => t.Id == request.Id, ct);

        if (template is null)
            return Result<MailTemplateDto>.Failure("Mail şablonu bulunamadı.");

        return Result<MailTemplateDto>.Success(new MailTemplateDto(
            template.Id,
            template.Name,
            template.Subject,
            template.HtmlBody,
            template.CreatedAt,
            template.UpdatedAt));
    }
}
