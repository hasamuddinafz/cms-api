using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiry,
    Guid UserId,
    string FullName,
    string Email,
    IReadOnlyList<string> Permissions
);
