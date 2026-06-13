using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace NexusCms.Api.Authorization;

/// <summary>
/// Dynamically creates an AuthorizationPolicy for any policy name that matches
/// a permission key (e.g. "pages.edit"), so we don't need to register every
/// permission manually at startup.
/// </summary>
public class PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallback = new(options);

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => _fallback.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => _fallback.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // If the policy name looks like a permission key, build it on the fly
        if (policyName.Contains('.'))
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return _fallback.GetPolicyAsync(policyName);
    }
}
