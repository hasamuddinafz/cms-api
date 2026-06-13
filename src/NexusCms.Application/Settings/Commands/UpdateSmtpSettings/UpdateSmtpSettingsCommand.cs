using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Settings.Commands.UpdateSmtpSettings;

/// <summary>
/// SmtpJson.password may arrive as the mask "••••••" (user didn't change it)
/// or as plaintext (user typed a new password). The handler handles both cases.
/// </summary>
public record UpdateSmtpSettingsCommand(string SmtpJson) : IRequest<Result<AppSettingDto>>;
