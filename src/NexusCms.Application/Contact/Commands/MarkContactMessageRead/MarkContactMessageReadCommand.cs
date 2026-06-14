using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Contact.Commands.MarkContactMessageRead;

public record MarkContactMessageReadCommand(Guid Id, bool IsRead = true) : IRequest<Result>;
