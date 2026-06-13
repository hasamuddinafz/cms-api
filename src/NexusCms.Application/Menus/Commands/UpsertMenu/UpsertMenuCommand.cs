using MediatR;
using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.Menus.Commands.UpsertMenu;

/// <summary>
/// Creates the menu record if it doesn't exist yet, otherwise replaces ItemsJson.
/// The four standard keys (navbar-desktop, navbar-mobile, footer-desktop, footer-mobile)
/// are seeded on startup; this command is the single write path for menu editors.
/// </summary>
public record UpsertMenuCommand(
    string Key,
    MenuType Type,
    string ItemsJson
) : IRequest<Result<Guid>>;
