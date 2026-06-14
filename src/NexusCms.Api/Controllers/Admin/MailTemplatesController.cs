using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Api.Authorization;
using NexusCms.Application.MailTemplates.Commands.CreateMailTemplate;
using NexusCms.Application.MailTemplates.Commands.DeleteMailTemplate;
using NexusCms.Application.MailTemplates.Commands.UpdateMailTemplate;
using NexusCms.Application.MailTemplates.Queries.GetMailTemplateById;
using NexusCms.Application.MailTemplates.Queries.GetMailTemplateList;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/mail-templates")]
[Authorize]
public class MailTemplatesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.MailTemplates.View)]
    public async Task<IActionResult> List(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var r = await mediator.Send(new GetMailTemplateListQuery(search, page, pageSize), ct);
        return r.IsSuccess ? Ok(r.Value) : BadRequest(new { error = r.Error });
    }

    [HttpGet("{id:guid}")]
    [RequirePermission(Permissions.MailTemplates.View)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var r = await mediator.Send(new GetMailTemplateByIdQuery(id), ct);
        return r.IsSuccess ? Ok(r.Value) : NotFound(new { error = r.Error });
    }

    [HttpPost]
    [RequirePermission(Permissions.MailTemplates.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMailTemplateCommand command, CancellationToken ct)
    {
        var r = await mediator.Send(command, ct);
        return r.IsSuccess ? Ok(new { id = r.Value }) : BadRequest(new { error = r.Error });
    }

    [HttpPut("{id:guid}")]
    [RequirePermission(Permissions.MailTemplates.Edit)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMailTemplateCommand command, CancellationToken ct)
    {
        var r = await mediator.Send(command with { Id = id }, ct);
        return r.IsSuccess ? NoContent() : BadRequest(new { error = r.Error });
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(Permissions.MailTemplates.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var r = await mediator.Send(new DeleteMailTemplateCommand(id), ct);
        return r.IsSuccess ? NoContent() : NotFound(new { error = r.Error });
    }
}
