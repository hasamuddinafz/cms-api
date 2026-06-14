using MediatR;
using NexusCms.Application.MailTemplates;
using NexusCms.Domain.Common;

namespace NexusCms.Application.MailTemplates.Queries.GetMailTemplateById;

public record GetMailTemplateByIdQuery(Guid Id) : IRequest<Result<MailTemplateDto>>;
