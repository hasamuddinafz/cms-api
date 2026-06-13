using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Settings.Commands.UpdateGeneralSettings;

public record UpdateGeneralSettingsCommand(string GeneralJson) : IRequest<Result<AppSettingDto>>;
