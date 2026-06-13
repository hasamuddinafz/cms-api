using Microsoft.AspNetCore.Identity;

namespace NexusCms.Domain.Entities;

/// <summary>
/// Extends IdentityUserRole to allow navigation properties.
/// A user can have multiple roles.
/// </summary>
public class UserRole : IdentityUserRole<Guid>
{
    public ApplicationUser User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
