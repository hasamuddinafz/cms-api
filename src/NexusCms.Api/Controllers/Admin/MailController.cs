using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Application.Mail.Commands.SendMail;
using NexusCms.Application.Mail.Commands.SendBulkMail;
using NexusCms.Api.Authorization;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/mail")]
[Authorize]
public class MailController(IMediator mediator) : ControllerBase
{
    [HttpPost("send")]
    [RequirePermission(Permissions.Mail.Send)]
    public async Task<IActionResult> Send([FromBody] SendMailCommand command, CancellationToken ct)
    {
        var r = await mediator.Send(command, ct);
        return r.IsSuccess ? Ok(new { message = "Mail gönderildi." }) : BadRequest(new { error = r.Error });
    }

    [HttpPost("send-bulk")]
    [RequirePermission(Permissions.Mail.Send)]
    public async Task<IActionResult> SendBulk([FromBody] SendBulkMailCommand command, CancellationToken ct)
    {
        var r = await mediator.Send(command, ct);
        return r.IsSuccess ? Ok(new { sent = r.Value }) : BadRequest(new { error = r.Error });
    }
}
