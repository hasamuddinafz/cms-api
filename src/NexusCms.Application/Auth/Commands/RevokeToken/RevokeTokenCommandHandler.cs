using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Auth.Commands.RevokeToken;

public class RevokeTokenCommandHandler(IDbContextAccessor db)
    : IRequestHandler<RevokeTokenCommand, Result>
{
    public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken ct)
    {
        var token = await db.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == request.Token, ct);

        if (token is null || !token.IsActive)
            return Result.Failure("Token not found or already revoked.");

        token.IsRevoked = true;
        token.RevokedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
