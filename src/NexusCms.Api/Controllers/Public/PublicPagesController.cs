using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NexusCms.Application.Public.Queries.GetPublicPage;
using NexusCms.Application.Public.Queries.GetPublicPageList;

namespace NexusCms.Api.Controllers.Public;

[ApiController]
[Route("api/public/pages")]
public class PublicPagesController(IMediator mediator) : ControllerBase
{
    /// <summary>Returns a list of published pages for a given model (e.g. blog index).</summary>
    [HttpGet("{modelKey}")]
    [OutputCache(PolicyName = "Public5min")]
    public async Task<IActionResult> GetList(
        string modelKey,
        [FromQuery] int page     = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetPublicPageListQuery(modelKey, page, pageSize), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    /// <summary>Returns a single published page — also triggers ViewCount increment.</summary>
    [HttpGet("{modelKey}/{slug}")]
    // No output cache here: each hit should increment ViewCount
    public async Task<IActionResult> GetBySlug(
        string modelKey,
        string slug,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetPublicPageQuery(modelKey, slug), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
