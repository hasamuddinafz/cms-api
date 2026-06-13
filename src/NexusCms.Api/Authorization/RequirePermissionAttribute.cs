using Microsoft.AspNetCore.Authorization;

namespace NexusCms.Api.Authorization;

/// <summary>
/// Declares that the decorated action/controller requires a specific permission claim in the JWT.
/// Usage: [RequirePermission(Permissions.Pages.Edit)]
/// </summary>
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permission)
        : base(policy: permission)
    {
        // Policy name == permission key; policies are registered dynamically via PermissionPolicyProvider
    }
}
