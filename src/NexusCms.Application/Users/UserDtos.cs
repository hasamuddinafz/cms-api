namespace NexusCms.Application.Users;

public record UserListItemDto(
    Guid     Id,
    string?  Email,
    string?  FullName,
    bool     IsActive,
    DateTime? LastLoginAt,
    IReadOnlyList<string> RoleNames);

public record UserDetailDto(
    Guid     Id,
    string?  Email,
    string?  FullName,
    bool     IsActive,
    DateTime? LastLoginAt,
    IReadOnlyList<RoleRefDto> Roles);

public record RoleRefDto(Guid Id, string Name);
