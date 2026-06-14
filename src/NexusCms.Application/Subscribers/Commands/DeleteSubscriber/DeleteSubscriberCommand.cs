using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Subscribers.Commands.DeleteSubscriber;

public record DeleteSubscriberCommand(Guid Id) : IRequest<Result>;
