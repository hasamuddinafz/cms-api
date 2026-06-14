using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Api.Authorization;
using NexusCms.Application.Users.Commands.AssignRoles;
using NexusCms.Application.Users.Commands.CreateUser;
using NexusCms.Application.Users.Queries.GetUserList;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.Users.Manage)]
    public async Task<IActionResult> GetList(
        [FromQuery] string search   = "",
        [FromQuery] int    page     = 1,
        [FromQuery] int    pageSize = 20,
        CancellationToken ct = default)
    {
        var r = await mediator.Send(new GetUserListQuery(search, page, pageSize), ct);
        return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
    }

    [HttpPost]
    [RequirePermission(Permissions.Users.Manage)]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        var r = await mediator.Send(command, ct);
        return r.IsSuccess ? Ok(new { id = r.Value }) : BadRequest(new { error = r.Error });
    }

    [HttpPut("{id:guid}/roles")]
    [RequirePermission(Permissions.Users.Manage)]
    public async Task<IActionResult> AssignRoles(Guid id, [FromBody] AssignRolesRequest req, CancellationToken ct)
    {
        var r = await mediator.Send(new AssignRolesCommand(id, req.RoleIds), ct);
        return r.IsSuccess ? NoContent() : BadRequest(r.Error);
    }
}

public record AssignRolesRequest(List<Guid> RoleIds);
