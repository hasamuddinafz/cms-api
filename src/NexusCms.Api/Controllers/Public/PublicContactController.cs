using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Contact.Commands.SubmitContactMessage;
using NexusCms.Application.Subscribers.Commands.Subscribe;
using NexusCms.Application.Subscribers.Commands.Unsubscribe;
using NexusCms.Persistence;
using System.Text.Json;

namespace NexusCms.Api.Controllers.Public;

[ApiController]
[Route("api/public")]
public class PublicContactController(IMediator mediator, AppDbContext db) : ControllerBase
{
    [HttpPost("contact")]
    public async Task<IActionResult> Contact([FromBody] ContactRequest body, CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var ua = Request.Headers.UserAgent.ToString();
        var r = await mediator.Send(new SubmitContactMessageCommand(
            body.FullName, body.Email, body.Phone, body.Subject, body.Message, ip, ua), ct);
        return r.IsSuccess
            ? Ok(new { message = "Mesajınız alındı." })
            : BadRequest(new { error = r.Error });
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest body, CancellationToken ct)
    {
        if (!await IsSubscriptionEnabledAsync(ct))
            return StatusCode(503, new { error = "Abonelik sistemi şu anda aktif değil." });

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var r = await mediator.Send(new SubscribeCommand(body.Email, body.FullName, ip), ct);
        return r.IsSuccess
            ? Ok(new { message = "Aboneliğiniz oluşturuldu." })
            : BadRequest(new { error = r.Error });
    }

    [HttpPost("unsubscribe")]
    public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribeRequest body, CancellationToken ct)
    {
        var r = await mediator.Send(new UnsubscribeCommand(body.Email), ct);
        return r.IsSuccess
            ? Ok(new { message = "Aboneliğiniz iptal edildi." })
            : BadRequest(new { error = r.Error });
    }

    private async Task<bool> IsSubscriptionEnabledAsync(CancellationToken ct)
    {
        var setting = await db.AppSettings.AsNoTracking().FirstOrDefaultAsync(ct);
        if (setting is null) return false;
        try
        {
            var general = JsonSerializer.Deserialize<JsonElement>(setting.GeneralJson);
            if (general.TryGetProperty("subscriptionEnabled", out var prop))
                return prop.GetBoolean();
        }
        catch { }
        return false;
    }

    public record ContactRequest(string FullName, string Email, string? Phone, string Subject, string Message);
    public record SubscribeRequest(string Email, string? FullName);
    public record UnsubscribeRequest(string Email);
}
