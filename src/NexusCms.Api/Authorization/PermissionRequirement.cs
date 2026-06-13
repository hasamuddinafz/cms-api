using Microsoft.AspNetCore.Authorization;

namespace NexusCms.Api.Authorization;

public record PermissionRequirement(string Permission) : IAuthorizationRequirement;
