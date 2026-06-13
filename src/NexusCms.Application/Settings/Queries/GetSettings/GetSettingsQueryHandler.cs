using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Settings.Queries.GetSettings;

public class GetSettingsQueryHandler(IDbContextAccessor db) : IRequestHandler<GetSettingsQuery, Result<AppSettingDto>>
{
    public async Task<Result<AppSettingDto>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        var setting = await db.AppSettings.FirstOrDefaultAsync(cancellationToken);

        if (setting is null)
        {
            // Return empty singleton placeholders
            setting = new AppSetting
            {
                GeneralJson      = "{}",
                SeoJson          = "{}",
                SmtpJson         = "{}",
                LocalizationJson = "{}",
                ThemeJson        = "{}"
            };
        }

        return Result<AppSettingDto>.Success(ToDto(setting));
    }

    internal static AppSettingDto ToDto(AppSetting s) => new(
        s.GeneralJson,
        s.SeoJson,
        SmtpJsonHelper.MaskPassword(s.SmtpJson),
        s.LocalizationJson,
        s.ThemeJson,
        s.UpdatedAt);
}
