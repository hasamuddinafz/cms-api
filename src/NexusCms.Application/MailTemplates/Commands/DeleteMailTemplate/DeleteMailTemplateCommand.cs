using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.MailTemplates.Commands.DeleteMailTemplate;

public record DeleteMailTemplateCommand(Guid Id) : IRequest<Result>;
