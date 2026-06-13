namespace NexusCms.Application.Roles;

public record RoleDto(
    Guid     Id,
    string   Name,
    string?  Description,
    bool     IsSystemRole,
    IReadOnlyList<string> PermissionKeys,
    DateTime CreatedAt);

public record RoleListItemDto(
    Guid    Id,
    string  Name,
    string? Description,
    bool    IsSystemRole,
    int     PermissionCount,
    DateTime CreatedAt);

public record PermissionGroupDto(
    string Module,
    IReadOnlyList<PermissionItemDto> Permissions);

public record PermissionItemDto(Guid Id, string Key, string Label);
