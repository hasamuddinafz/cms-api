using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Api.Authorization;
using NexusCms.Application.Common.Models;
using NexusCms.Application.PageModels.Commands.CreatePageModel;
using NexusCms.Application.PageModels.Commands.DeletePageModel;
using NexusCms.Application.PageModels.Commands.UpdatePageModel;
using NexusCms.Application.PageModels.Queries.ExportPageModels;
using NexusCms.Application.PageModels.Queries.GetPageModelById;
using NexusCms.Application.PageModels.Queries.GetPageModelList;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/page-models")]
public class PageModelsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.PageModels.View)]
    public async Task<IActionResult> List([FromQuery] PagedRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPageModelListQuery(request), ct);
        return Ok(result);
    }

    [HttpGet("export")]
    [RequirePermission(Permissions.PageModels.View)]
    public async Task<IActionResult> Export([FromQuery] PagedRequest request, [FromQuery] string format = "csv", CancellationToken ct = default)
    {
        var result = await mediator.Send(new ExportPageModelsQuery(request, format), ct);
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission(Permissions.PageModels.View)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPageModelByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }

    [HttpPost]
    [RequirePermission(Permissions.PageModels.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePageModelCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value })
            : BadRequest(new { error = result.Error });
    }

    [HttpPut("{id:guid}")]
    [RequirePermission(Permissions.PageModels.Edit)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePageModelCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { Id = id }, ct);
        return result.IsSuccess ? NoContent() : NotFound(new { error = result.Error });
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(Permissions.PageModels.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeletePageModelCommand(id), ct);
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }
}
