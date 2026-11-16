using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NexusCms.Application.Auth.Commands.Login;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IConfiguration configuration,
    IDbContextAccessor db) : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var stored = await db.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == request.Token, ct);

        if (stored is null || !stored.IsActive)
            return Result.Failure<LoginResponse>("Invalid or expired refresh token.");

        if (stored.ExpiresAt <= DateTime.UtcNow)
            return Result.Failure<LoginResponse>("Session expired. Please sign in again.");

        if (!stored.User.IsActive)
            return Result.Failure<LoginResponse>("User account is disabled.");

        // Rotate: revoke old, issue new
        stored.IsRevoked  = true;
        stored.RevokedAt  = DateTime.UtcNow;

        var permissions  = await db.GetUserPermissionsAsync(stored.UserId, ct);
        var accessToken  = jwtTokenGenerator.GenerateAccessToken(stored.User, permissions);
        var newRefresh   = jwtTokenGenerator.GenerateRefreshToken();

        var refreshDays  = configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7);
        await db.RefreshTokens.AddAsync(new Domain.Entities.RefreshToken
        {
            UserId    = stored.UserId,
            Token     = newRefresh,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshDays)
        }, ct);

        await db.SaveChangesAsync(ct);

        var accessMinutes = configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15);

        return Result.Success(new LoginResponse(
            accessToken,
            newRefresh,
            DateTime.UtcNow.AddMinutes(accessMinutes),
            stored.UserId,
            stored.User.FullName,
            stored.User.Email!,
            permissions
        ));
    }
}
