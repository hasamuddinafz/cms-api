using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Settings.Commands.UpdateLocalizationSettings;

public record UpdateLocalizationSettingsCommand(string LocalizationJson) : IRequest<Result<AppSettingDto>>;
