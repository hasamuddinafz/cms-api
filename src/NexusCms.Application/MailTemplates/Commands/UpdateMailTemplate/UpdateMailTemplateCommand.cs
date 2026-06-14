using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.MailTemplates.Commands.UpdateMailTemplate;

public record UpdateMailTemplateCommand(Guid Id, string Name, string Subject, string HtmlBody) : IRequest<Result>;
