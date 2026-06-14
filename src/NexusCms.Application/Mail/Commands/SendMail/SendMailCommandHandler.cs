using MediatR;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Mail.Commands.SendMail;

public class SendMailCommandHandler(IEmailService emailService) : IRequestHandler<SendMailCommand, Result>
{
    public async Task<Result> Handle(SendMailCommand request, CancellationToken ct)
    {
        try
        {
            await emailService.SendAsync(request.ToEmail, request.ToName, request.Subject, request.HtmlBody, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Mail gönderilemedi: {ex.Message}");
        }
    }
}
