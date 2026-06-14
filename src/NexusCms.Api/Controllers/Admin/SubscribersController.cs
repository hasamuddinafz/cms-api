using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Application.Subscribers.Commands.DeleteSubscriber;
using NexusCms.Application.Subscribers.Queries.GetSubscribers;
using NexusCms.Api.Authorization;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/subscribers")]
[Authorize]
public class SubscribersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.Subscribers.View)]
    public async Task<IActionResult> List(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        [FromQuery] string sortBy = "subscribedAt",
        [FromQuery] string sortDir = "desc",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var r = await mediator.Send(new GetSubscribersQuery(search, isActive, sortBy, sortDir, page, pageSize), ct);
        return Ok(r);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(Permissions.Subscribers.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var r = await mediator.Send(new DeleteSubscriberCommand(id), ct);
        return r.IsSuccess ? NoContent() : NotFound(new { error = r.Error });
    }
}
