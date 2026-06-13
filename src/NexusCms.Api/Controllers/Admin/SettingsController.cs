using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Api.Authorization;
using NexusCms.Application.Settings;
using NexusCms.Application.Settings.Commands.UpdateGeneralSettings;
using NexusCms.Application.Settings.Commands.UpdateLocalizationSettings;
using NexusCms.Application.Settings.Commands.UpdateSeoSettings;
using NexusCms.Application.Settings.Commands.UpdateSmtpSettings;
using NexusCms.Application.Settings.Commands.UpdateThemeSettings;
using NexusCms.Application.Settings.Queries.GetSettings;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/settings")]
public class SettingsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.Settings.GeneralEdit)]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await mediator.Send(new GetSettingsQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("general")]
    [RequirePermission(Permissions.Settings.GeneralEdit)]
    public async Task<IActionResult> UpdateGeneral([FromBody] UpdateGeneralSettingsCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("seo")]
    [RequirePermission(Permissions.Settings.SeoEdit)]
    public async Task<IActionResult> UpdateSeo([FromBody] UpdateSeoSettingsCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("smtp")]
    [RequirePermission(Permissions.Settings.SmtpEdit)]
    public async Task<IActionResult> UpdateSmtp([FromBody] UpdateSmtpSettingsCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("localization")]
    [RequirePermission(Permissions.Settings.LocalizationEdit)]
    public async Task<IActionResult> UpdateLocalization([FromBody] UpdateLocalizationSettingsCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("theme")]
    [RequirePermission(Permissions.Settings.ThemeEdit)]
    public async Task<IActionResult> UpdateTheme([FromBody] UpdateThemeSettingsCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
