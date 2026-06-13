using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Api.Authorization;
using NexusCms.Application.Dashboard.Queries.GetDashboardStats;
using NexusCms.Application.Dashboard.Queries.GetRecentActivity;
using NexusCms.Application.Dashboard.Queries.GetTopPages;
using NexusCms.Application.Dashboard.Queries.GetViewTrend;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/dashboard")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    [HttpGet("stats")]
    [RequirePermission(Permissions.Dashboard.View)]
    public async Task<IActionResult> Stats(CancellationToken ct)
    {
        var r = await mediator.Send(new GetDashboardStatsQuery(), ct);
        return r.IsSuccess ? Ok(r.Value) : StatusCode(500, r.Error);
    }

    [HttpGet("trend")]
    [RequirePermission(Permissions.Dashboard.View)]
    public async Task<IActionResult> Trend([FromQuery] int days = 30, CancellationToken ct = default)
    {
        var r = await mediator.Send(new GetViewTrendQuery(days), ct);
        return r.IsSuccess ? Ok(r.Value) : StatusCode(500, r.Error);
    }

    [HttpGet("top-pages")]
    [RequirePermission(Permissions.Dashboard.View)]
    public async Task<IActionResult> TopPages([FromQuery] int take = 10, CancellationToken ct = default)
    {
        var r = await mediator.Send(new GetTopPagesQuery(take), ct);
        return r.IsSuccess ? Ok(r.Value) : StatusCode(500, r.Error);
    }

    [HttpGet("recent-activity")]
    [RequirePermission(Permissions.Dashboard.View)]
    public async Task<IActionResult> RecentActivity([FromQuery] int take = 15, CancellationToken ct = default)
    {
        var r = await mediator.Send(new GetRecentActivityQuery(take), ct);
        return r.IsSuccess ? Ok(r.Value) : StatusCode(500, r.Error);
    }
}
