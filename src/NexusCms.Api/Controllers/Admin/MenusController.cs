using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Api.Authorization;
using NexusCms.Application.Menus.Commands.UpsertMenu;
using NexusCms.Application.Menus.Queries.GetMenuByKey;
using NexusCms.Application.Menus.Queries.GetMenuList;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/menus")]
public class MenusController(IMediator mediator) : ControllerBase
{
    /// <summary>Returns all four menu records (even empty ones).</summary>
    [HttpGet]
    [RequirePermission(Permissions.Menus.Manage)]
    public async Task<IActionResult> List(CancellationToken ct)
        => Ok(await mediator.Send(new GetMenuListQuery(), ct));

    /// <summary>GET /api/admin/menus/{key} — e.g. navbar-desktop</summary>
    [HttpGet("{key}")]
    [RequirePermission(Permissions.Menus.Manage)]
    public async Task<IActionResult> GetByKey(string key, CancellationToken ct)
    {
        var result = await mediator.Send(new GetMenuByKeyQuery(key), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }

    /// <summary>
    /// PUT /api/admin/menus/{key} — create-or-replace the full ItemsJson tree.
    /// The drag-and-drop editor sends the entire rebuilt tree on every save.
    /// </summary>
    [HttpPut("{key}")]
    [RequirePermission(Permissions.Menus.Manage)]
    public async Task<IActionResult> Upsert(string key, [FromBody] UpsertMenuBody body, CancellationToken ct)
    {
        var command = new UpsertMenuCommand(key, body.Type, body.ItemsJson);
        var result  = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(new { id = result.Value }) : BadRequest(new { error = result.Error });
    }
}

public record UpsertMenuBody(NexusCms.Domain.Enums.MenuType Type, string ItemsJson);
