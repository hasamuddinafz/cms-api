using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Settings.Queries.GetSettings;

public record GetSettingsQuery : IRequest<Result<AppSettingDto>>;
