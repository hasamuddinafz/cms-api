using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Application.Contact.Commands.DeleteContactMessage;
using NexusCms.Application.Contact.Commands.MarkContactMessageRead;
using NexusCms.Application.Contact.Queries.GetContactMessages;
using NexusCms.Application.Mail.Commands.SendMail;
using NexusCms.Api.Authorization;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/contact")]
[Authorize]
public class ContactController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.Contact.View)]
    public async Task<IActionResult> List(
        [FromQuery] string? search,
        [FromQuery] bool? isRead,
        [FromQuery] string sortBy = "createdAt",
        [FromQuery] string sortDir = "desc",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var r = await mediator.Send(new GetContactMessagesQuery(search, isRead, sortBy, sortDir, page, pageSize), ct);
        return Ok(r);
    }

    [HttpPut("{id:guid}/read")]
    [RequirePermission(Permissions.Contact.View)]
    public async Task<IActionResult> MarkRead(Guid id, [FromBody] MarkReadRequest body, CancellationToken ct)
    {
        var r = await mediator.Send(new MarkContactMessageReadCommand(id, body.IsRead), ct);
        return r.IsSuccess ? Ok() : NotFound(new { error = r.Error });
    }

    [HttpPost("{id:guid}/reply")]
    [RequirePermission(Permissions.Contact.Reply)]
    public async Task<IActionResult> Reply(Guid id, [FromBody] ReplyRequest body, CancellationToken ct)
    {
        // Mark as read automatically on reply
        await mediator.Send(new MarkContactMessageReadCommand(id, true), ct);
        var r = await mediator.Send(new SendMailCommand(body.ToEmail, body.ToName, body.Subject, body.HtmlBody), ct);
        return r.IsSuccess ? Ok(new { message = "Reply sent." }) : BadRequest(new { error = r.Error });
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(Permissions.Contact.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var r = await mediator.Send(new DeleteContactMessageCommand(id), ct);
        return r.IsSuccess ? NoContent() : NotFound(new { error = r.Error });
    }

    public record MarkReadRequest(bool IsRead);
    public record ReplyRequest(string ToEmail, string ToName, string Subject, string HtmlBody);
}
