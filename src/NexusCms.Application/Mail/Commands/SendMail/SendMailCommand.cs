using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Mail.Commands.SendMail;

public record SendMailCommand(
    string ToEmail,
    string ToName,
    string Subject,
    string HtmlBody
) : IRequest<Result>;
