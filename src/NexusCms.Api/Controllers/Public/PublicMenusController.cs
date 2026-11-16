using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NexusCms.Application.Public.Queries.GetPublicMenu;

namespace NexusCms.Api.Controllers.Public;

[ApiController]
[Route("api/public/menus")]
public class PublicMenusController(IMediator mediator) : ControllerBase
{
    [HttpGet("{key}")]
    [OutputCache(PolicyName = "Public5min")]
    public async Task<IActionResult> Get(string key, [FromQuery] string lang = "en", CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetPublicMenuQuery(key, lang), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
