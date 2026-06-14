using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Subscribers.Commands.Subscribe;

public record SubscribeCommand(string Email, string? FullName, string? IpAddress) : IRequest<Result<Guid>>;
