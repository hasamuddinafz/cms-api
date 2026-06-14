using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Subscribers.Commands.Unsubscribe;

public record UnsubscribeCommand(string Email) : IRequest<Result>;
