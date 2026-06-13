using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Auth.Commands.RevokeToken;

public record RevokeTokenCommand(string Token) : IRequest<Result>;
