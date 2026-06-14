using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Mail.Commands.SendBulkMail;

public class SendBulkMailHandler(IDbContextAccessor db, IEmailService emailService)
    : IRequestHandler<SendBulkMailCommand, Result<int>>
{
    public async Task<Result<int>> Handle(SendBulkMailCommand request, CancellationToken ct)
    {
        var subscribers = await db.Subscribers
            .Where(s => s.IsActive)
            .Select(s => new { s.Email, s.FullName })
            .ToListAsync(ct);

        var tasks = subscribers.Select(async s =>
        {
            try
            {
                await emailService.SendAsync(s.Email, s.FullName ?? string.Empty, request.Subject, request.HtmlBody, ct);
                return true;
            }
            catch
            {
                return false;
            }
        });

        var results = await Task.WhenAll(tasks);
        var successCount = results.Count(r => r);

        return Result<int>.Success(successCount);
    }
}
