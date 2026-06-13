using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NexusCms.Application.Public.Queries.GetPublicSettings;

namespace NexusCms.Api.Controllers.Public;

[ApiController]
[Route("api/public/settings")]
public class PublicSettingsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = "Public5min")]
    public async Task<IActionResult> Get([FromQuery] string lang = "tr", CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetPublicSettingsQuery(lang), ct);
        return result.IsSuccess ? Ok(result.Value) : StatusCode(500, result.Error);
    }
}
