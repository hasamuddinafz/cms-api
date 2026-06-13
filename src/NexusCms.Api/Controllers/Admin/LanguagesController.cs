using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Api.Authorization;
using NexusCms.Application.Languages.Commands.CreateLanguage;
using NexusCms.Application.Languages.Commands.DeleteLanguage;
using NexusCms.Application.Languages.Commands.UpdateLanguage;
using NexusCms.Application.Languages.Queries.GetLanguageList;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/languages")]
public class LanguagesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.Settings.LanguagesManage)]
    public async Task<IActionResult> GetList([FromQuery] bool activeOnly = false, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetLanguageListQuery(activeOnly), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    [RequirePermission(Permissions.Settings.LanguagesManage)]
    public async Task<IActionResult> Create([FromBody] CreateLanguageCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetList), result.Value) : BadRequest(result.Error);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission(Permissions.Settings.LanguagesManage)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLanguageRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateLanguageCommand(
            id, req.Name, req.IsDefault, req.IsActive, req.FlagIcon, req.SortOrder), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(Permissions.Settings.LanguagesManage)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteLanguageCommand(id), ct);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}

public record UpdateLanguageRequest(string Name, bool IsDefault, bool IsActive, string FlagIcon, int SortOrder);
