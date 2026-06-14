using System.Text.Json;
using NexusCms.Application.Common.Interfaces;
using MediatR;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;


namespace NexusCms.Application.Contact.Commands.SubmitContactMessage;

public class SubmitContactMessageHandler(IDbContextAccessor db, IEmailService emailService) : IRequestHandler<SubmitContactMessageCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SubmitContactMessageCommand request, CancellationToken ct)
    {
        var msg = new ContactMessage
        {
            FullName  = request.FullName,
            Email     = request.Email,
            Phone     = request.Phone,
            Subject   = request.Subject,
            Message   = request.Message,
            IpAddress = request.IpAddress,
            UserAgent = request.UserAgent,
        };
        db.ContactMessages.Add(msg);
        await db.SaveChangesAsync(ct);

        // Auto-reply logic
        var appSetting = await db.AppSettings.FindAsync([AppSetting.SingletonId], ct);
        if (appSetting is not null)
        {
            try
            {
                using var doc = JsonDocument.Parse(appSetting.GeneralJson);
                var root = doc.RootElement;
                if (root.TryGetProperty("contactAutoReplyEnabled", out var enabledProp) && enabledProp.GetBoolean())
                {
                    var replySubject = root.TryGetProperty("contactAutoReplySubject", out var subProp)
                        ? subProp.GetString() ?? string.Empty
                        : string.Empty;
                    var replyBody = root.TryGetProperty("contactAutoReplyBody", out var bodyProp)
                        ? bodyProp.GetString() ?? string.Empty
                        : string.Empty;

                    _ = emailService.SendAsync(request.Email, request.FullName, replySubject, replyBody);
                }
            }
            catch
            {
                // fire-and-forget — don't fail the contact form submission on mail errors
            }
        }

        return Result.Success(msg.Id);
    }
}
