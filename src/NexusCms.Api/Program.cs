using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexusCms.Api;
using NexusCms.Application;
using NexusCms.Domain;
using NexusCms.Domain.Entities;
using NexusCms.Persistence;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .WriteTo.Console()
        .WriteTo.File("logs/nexus-.log", rollingInterval: RollingInterval.Day));

    builder.Services
        .AddDomain()
        .AddApplication()
        .AddPersistence(builder.Configuration)
        .AddApi(builder.Configuration);

    // Serialize enums (PageStatus, FileType, …) as their names — the panel compares against strings
    builder.Services.AddControllers()
        .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    // Ensure wwwroot exists so FileStorageService can write to it
    var wwwroot = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
    Directory.CreateDirectory(wwwroot);
    builder.Environment.WebRootPath = wwwroot;

    var app = builder.Build();

    // Auto-migrate + seed on startup
    using (var scope = app.Services.CreateScope())
    {
        var db          = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        await db.Database.MigrateAsync();
        await DatabaseSeeder.SeedAsync(db, userManager, roleManager);
    }

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NEXUS CMS API v1"));

    app.UseSerilogRequestLogging();
    app.UseMiddleware<NexusCms.Api.Middleware.ExceptionMiddleware>();
    app.UseStaticFiles();   // serves /uploads/* from wwwroot
    app.UseHttpsRedirection();
    app.UseCors("Panel");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseOutputCache();
    app.UseRateLimiter();
    app.MapControllers();
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
