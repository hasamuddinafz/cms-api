using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NexusCms.Api.Authorization;
using NexusCms.Api.Services;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Options;
using Microsoft.Extensions.Logging;

namespace NexusCms.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        // JWT Bearer
        var jwtKey = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key is not configured.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey  = true,
                    ValidIssuer              = configuration["Jwt:Issuer"],
                    ValidAudience            = configuration["Jwt:Audience"],
                    IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew                = TimeSpan.Zero
                };
            });

        // Dynamic permission policy provider
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthorization();

        // Application services
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddSingleton<IEncryptionService, AesEncryptionService>();
        services.AddHttpContextAccessor();

        // Options
        services.Configure<FileStorageOptions>(configuration.GetSection(FileStorageOptions.SectionName));

        // PageView background writer — singleton acts as both IPageViewQueue and BackgroundService
        services.AddSingleton<PageViewBackgroundService>();
        services.AddSingleton<IPageViewQueue>(sp => sp.GetRequiredService<PageViewBackgroundService>());
        services.AddHostedService(sp => sp.GetRequiredService<PageViewBackgroundService>());

        // Output caching
        services.AddOutputCache(opts =>
        {
            opts.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(60)));
            opts.AddPolicy("Public5min", b => b.Expire(TimeSpan.FromMinutes(5)));
        });

        return services;
    }
}
