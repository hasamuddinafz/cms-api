using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.MailTemplates.Commands.CreateMailTemplate;

public record CreateMailTemplateCommand(string Name, string Subject, string HtmlBody) : IRequest<Result<Guid>>;
