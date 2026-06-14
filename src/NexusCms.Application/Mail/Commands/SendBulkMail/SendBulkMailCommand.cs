using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Mail.Commands.SendBulkMail;

public record SendBulkMailCommand(string Subject, string HtmlBody) : IRequest<Result<int>>;
