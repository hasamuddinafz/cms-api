using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Api.Authorization;
using NexusCms.Application.Roles.Commands.CreateRole;
using NexusCms.Application.Roles.Commands.DeleteRole;
using NexusCms.Application.Roles.Commands.UpdateRole;
using NexusCms.Application.Roles.Queries.GetAllPermissions;
using NexusCms.Application.Roles.Queries.GetRoleById;
using NexusCms.Application.Roles.Queries.GetRoleList;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/roles")]
public class RolesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.Roles.Manage)]
    public async Task<IActionResult> GetList([FromQuery] string search = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var r = await mediator.Send(new GetRoleListQuery(search, page, pageSize), ct);
        return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
    }

    [HttpGet("permissions")]
    [RequirePermission(Permissions.Roles.Manage)]
    public async Task<IActionResult> GetAllPermissions(CancellationToken ct)
    {
        var r = await mediator.Send(new GetAllPermissionsQuery(), ct);
        return r.IsSuccess ? Ok(r.Value) : StatusCode(500, r.Error);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission(Permissions.Roles.Manage)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var r = await mediator.Send(new GetRoleByIdQuery(id), ct);
        return r.IsSuccess ? Ok(r.Value) : NotFound(r.Error);
    }

    [HttpPost]
    [RequirePermission(Permissions.Roles.Manage)]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand cmd, CancellationToken ct)
    {
        var r = await mediator.Send(cmd, ct);
        return r.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = r.Value!.Id }, r.Value) : BadRequest(r.Error);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission(Permissions.Roles.Manage)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleRequest req, CancellationToken ct)
    {
        var r = await mediator.Send(new UpdateRoleCommand(id, req.Name, req.Description, req.PermissionIds), ct);
        return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(Permissions.Roles.Manage)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var r = await mediator.Send(new DeleteRoleCommand(id), ct);
        return r.IsSuccess ? NoContent() : BadRequest(r.Error);
    }
}

public record UpdateRoleRequest(string Name, string? Description, List<Guid> PermissionIds);
