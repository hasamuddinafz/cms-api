using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NexusCms.Application.Common.Interfaces;

namespace NexusCms.Api.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var value = Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? Principal?.FindFirstValue("sub");
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public string? Email => Principal?.FindFirstValue(ClaimTypes.Email)
                         ?? Principal?.FindFirstValue("email");

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public IReadOnlyList<string> Permissions =>
        Principal?.FindAll("permission").Select(c => c.Value).ToList() ?? [];

    public bool HasPermission(string permissionKey) =>
        Permissions.Contains(permissionKey);

    private HttpContext? Ctx => httpContextAccessor.HttpContext;

    public string? IpAddress =>
        Ctx?.Connection.RemoteIpAddress?.ToString();

    public string? UserAgent =>
        Ctx?.Request.Headers.UserAgent.FirstOrDefault();

    public string? Referrer =>
        Ctx?.Request.Headers.Referer.FirstOrDefault();
}
