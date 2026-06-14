using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Contact.Commands.DeleteContactMessage;

public record DeleteContactMessageCommand(Guid Id) : IRequest<Result>;
