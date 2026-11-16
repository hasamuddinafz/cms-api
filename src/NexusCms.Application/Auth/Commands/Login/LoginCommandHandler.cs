using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Auth.Commands.Login;

public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator jwtTokenGenerator,
    IConfiguration configuration,
    IDbContextAccessor db) : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.IsActive)
            return Result.Failure<LoginResponse>("Invalid email or password.");

        var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
            return Result.Failure<LoginResponse>("Invalid email or password.");

        var permissions = await db.GetUserPermissionsAsync(user.Id, ct);

        var accessToken  = jwtTokenGenerator.GenerateAccessToken(user, permissions);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        var refreshDays = configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7);
        var refreshTokenEntity = new Domain.Entities.RefreshToken
        {
            UserId    = user.Id,
            Token     = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshDays)
        };

        await db.RefreshTokens.AddAsync(refreshTokenEntity, ct);

        user.LastLoginAt = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        await db.SaveChangesAsync(ct);

        var accessMinutes = configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15);

        return Result.Success(new LoginResponse(
            accessToken,
            refreshToken,
            DateTime.UtcNow.AddMinutes(accessMinutes),
            user.Id,
            user.FullName,
            user.Email!,
            permissions
        ));
    }
}
