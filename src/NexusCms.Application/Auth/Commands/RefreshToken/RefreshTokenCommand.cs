using MediatR;
using NexusCms.Application.Auth.Commands.Login;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<Result<LoginResponse>>;
