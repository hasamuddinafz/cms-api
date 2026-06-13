using NexusCms.Domain.Enums;

namespace NexusCms.Application.Menus;

public record MenuDto(
    Guid Id,
    string Key,
    MenuType Type,
    string ItemsJson,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

/// <summary>
/// Well-known menu keys. The four records are seeded on first startup.
/// </summary>
public static class MenuKeys
{
    public const string NavbarDesktop  = "navbar-desktop";
    public const string NavbarMobile   = "navbar-mobile";
    public const string FooterDesktop  = "footer-desktop";
    public const string FooterMobile   = "footer-mobile";

    public static readonly IReadOnlyList<(string Key, MenuType Type)> All =
    [
        (NavbarDesktop, MenuType.Header),
        (NavbarMobile,  MenuType.Header),
        (FooterDesktop, MenuType.Footer),
        (FooterMobile,  MenuType.Footer),
    ];
}
