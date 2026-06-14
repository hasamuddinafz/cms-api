using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Api.Authorization;
using NexusCms.Application.Common.Models;
using NexusCms.Application.PageInstances.Commands.ChangePageInstanceStatus;
using NexusCms.Application.PageInstances.Commands.CreatePageInstance;
using NexusCms.Application.PageInstances.Commands.DeletePageInstance;
using NexusCms.Application.PageInstances.Commands.UpdatePageInstance;
using NexusCms.Application.PageInstances.Queries.ExportPageInstances;
using NexusCms.Application.PageInstances.Queries.GetPageInstanceById;
using NexusCms.Application.PageInstances.Queries.GetPageInstanceList;
using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/pages")]
public class PageInstancesController(IMediator mediator) : ControllerBase
{
    // ── List ─────────────────────────────────────────────────────────────────

    [HttpGet]
    [RequirePermission(Permissions.Pages.View)]
    public async Task<IActionResult> List(
        [FromQuery] PagedRequest request,
        [FromQuery] Guid? pageModelId,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetPageInstanceListQuery(request, pageModelId), ct);
        return Ok(result);
    }

    [HttpGet("export")]
    [RequirePermission(Permissions.Pages.View)]
    public async Task<IActionResult> Export(
        [FromQuery] PagedRequest request,
        [FromQuery] string format = "csv",
        [FromQuery] Guid? pageModelId = null,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new ExportPageInstancesQuery(request, format, pageModelId), ct);
        return File(result.Content, result.ContentType, result.FileName);
    }

    // ── Single ───────────────────────────────────────────────────────────────

    [HttpGet("{id:guid}")]
    [RequirePermission(Permissions.Pages.View)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPageInstanceByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }

    // ── Create / Update / Delete ─────────────────────────────────────────────

    [HttpPost]
    [RequirePermission(Permissions.Pages.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePageInstanceCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value })
            : BadRequest(new { error = result.Error });
    }

    [HttpPut("{id:guid}")]
    [RequirePermission(Permissions.Pages.Edit)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePageInstanceCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { Id = id }, ct);
        return result.IsSuccess ? NoContent() : NotFound(new { error = result.Error });
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(Permissions.Pages.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeletePageInstanceCommand(id), ct);
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }

    // ── Status transitions ───────────────────────────────────────────────────

    [HttpPost("{id:guid}/publish")]
    [RequirePermission(Permissions.Pages.Publish)]
    public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new ChangePageInstanceStatusCommand(id, PageStatus.Published), ct);
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }

    [HttpPost("{id:guid}/archive")]
    [RequirePermission(Permissions.Pages.Publish)]   // archive is also a publish-tier action
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new ChangePageInstanceStatusCommand(id, PageStatus.Archived), ct);
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }

    [HttpPost("{id:guid}/unpublish")]
    [RequirePermission(Permissions.Pages.Publish)]
    public async Task<IActionResult> Unpublish(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new ChangePageInstanceStatusCommand(id, PageStatus.Draft), ct);
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }
}
