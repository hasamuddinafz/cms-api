using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Contact.Commands.SubmitContactMessage;

public record SubmitContactMessageCommand(
    string FullName,
    string Email,
    string? Phone,
    string Subject,
    string Message,
    string? IpAddress,
    string? UserAgent
) : IRequest<Result<Guid>>;
