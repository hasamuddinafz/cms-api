using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Settings.Commands.UpdateSeoSettings;

public record UpdateSeoSettingsCommand(string SeoJson) : IRequest<Result<AppSettingDto>>;
