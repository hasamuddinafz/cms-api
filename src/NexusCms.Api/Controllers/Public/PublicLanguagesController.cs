using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NexusCms.Application.Public.Queries.GetPublicLanguages;

namespace NexusCms.Api.Controllers.Public;

[ApiController]
[Route("api/public/languages")]
public class PublicLanguagesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = "Public5min")]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await mediator.Send(new GetPublicLanguagesQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : StatusCode(500, result.Error);
    }
}
