using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace NexusCms.Api.Services;

public class SmtpEmailService(AppDbContext db, IEncryptionService encryption, ILogger<SmtpEmailService> logger)
    : IEmailService
{
    public async Task SendAsync(string toEmail, string toName, string subject, string htmlBody, CancellationToken ct = default)
    {
        var setting = await db.AppSettings.FirstOrDefaultAsync(ct)
            ?? throw new InvalidOperationException("AppSetting not found.");

        var smtp = JsonSerializer.Deserialize<SmtpConfig>(setting.SmtpJson) ?? new SmtpConfig();

        if (string.IsNullOrWhiteSpace(smtp.Host))
            throw new InvalidOperationException("SMTP settings are not configured.");

        var password = string.IsNullOrWhiteSpace(smtp.Password)
            ? string.Empty
            : encryption.Decrypt(smtp.Password);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(smtp.FromName ?? smtp.FromEmail, smtp.FromEmail));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        var secureOption = smtp.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable;
        await client.ConnectAsync(smtp.Host, smtp.Port > 0 ? smtp.Port : 587, secureOption, ct);

        if (!string.IsNullOrWhiteSpace(smtp.Username))
            await client.AuthenticateAsync(smtp.Username, password, ct);

        await client.SendAsync(message, ct);
        await client.DisconnectAsync(true, ct);

        logger.LogInformation("Email sent to {Email}, subject: {Subject}", toEmail, subject);
    }

    private sealed class SmtpConfig
    {
        public string? Host { get; set; }
        public int Port { get; set; } = 587;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FromEmail { get; set; }
        public string? FromName { get; set; }
        public bool UseSsl { get; set; }
    }
}
