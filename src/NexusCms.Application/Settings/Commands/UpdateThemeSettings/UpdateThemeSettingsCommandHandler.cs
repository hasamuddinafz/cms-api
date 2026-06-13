using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Settings.Commands.UpdateThemeSettings;

public class UpdateThemeSettingsCommandHandler(IDbContextAccessor db, IUnitOfWork uow)
    : IRequestHandler<UpdateThemeSettingsCommand, Result<AppSettingDto>>
{
    public async Task<Result<AppSettingDto>> Handle(UpdateThemeSettingsCommand request, CancellationToken cancellationToken)
    {
        var setting = await db.AppSettings.FirstOrDefaultAsync(cancellationToken)
            ?? CreateSingleton(db);

        setting.ThemeJson = request.ThemeJson;
        await uow.SaveChangesAsync(cancellationToken);

        return Result<AppSettingDto>.Success(Queries.GetSettings.GetSettingsQueryHandler.ToDto(setting));
    }

    private static AppSetting CreateSingleton(IDbContextAccessor db)
    {
        var s = new AppSetting { GeneralJson = "{}", SeoJson = "{}", SmtpJson = "{}", LocalizationJson = "{}", ThemeJson = "{}" };
        db.AppSettings.Add(s);
        return s;
    }
}
