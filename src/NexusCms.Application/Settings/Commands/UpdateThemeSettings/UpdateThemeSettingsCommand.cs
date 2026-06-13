using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Settings.Commands.UpdateThemeSettings;

public record UpdateThemeSettingsCommand(string ThemeJson) : IRequest<Result<AppSettingDto>>;
