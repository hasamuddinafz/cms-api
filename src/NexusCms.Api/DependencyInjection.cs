using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NexusCms.Api.Authorization;
using NexusCms.Api.Services;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Options;
using Microsoft.Extensions.Logging;
using NexusCms.Persistence;

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
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddSingleton<IEncryptionService, AesEncryptionService>();
        services.AddHttpContextAccessor();

        // Options
        services.Configure<FileStorageOptions>(configuration.GetSection(FileStorageOptions.SectionName));

        // PageView background writer — singleton acts as both IPageViewQueue and BackgroundService
        services.AddSingleton<PageViewBackgroundService>();
        services.AddSingleton<IPageViewQueue>(sp => sp.GetRequiredService<PageViewBackgroundService>());
        services.AddHostedService(sp => sp.GetRequiredService<PageViewBackgroundService>());

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "NEXUS CMS API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name         = "Authorization",
                Type         = SecuritySchemeType.Http,
                Scheme       = "bearer",
                BearerFormat = "JWT",
                In           = ParameterLocation.Header,
                Description  = "JWT token girin: Bearer {token}"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id   = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // CORS — origins must be explicitly configured; no silent localhost fallback
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? throw new InvalidOperationException("Cors:AllowedOrigins must be configured in appsettings.");

        services.AddCors(opts =>
        {
            opts.AddPolicy("Panel", policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // required for the refresh token httpOnly cookie
            });
        });

        // Output caching
        services.AddOutputCache(opts =>
        {
            opts.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(60)));
            opts.AddPolicy("Public5min", b => b.Expire(TimeSpan.FromMinutes(5)));
        });

        // Health checks
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>("database");

        // Rate limiting — 5 login attempts per minute per client
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("login", opt =>
            {
                opt.PermitLimit = 5;
                opt.Window = TimeSpan.FromMinutes(1);
                opt.QueueLimit = 0;
            });
            options.RejectionStatusCode = 429;
        });

        return services;
    }
}
