using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Settings.Commands.UpdateLocalizationSettings;

public class UpdateLocalizationSettingsCommandHandler(IDbContextAccessor db, IUnitOfWork uow)
    : IRequestHandler<UpdateLocalizationSettingsCommand, Result<AppSettingDto>>
{
    public async Task<Result<AppSettingDto>> Handle(UpdateLocalizationSettingsCommand request, CancellationToken cancellationToken)
    {
        var setting = await db.AppSettings.FirstOrDefaultAsync(cancellationToken)
            ?? CreateSingleton(db);

        setting.LocalizationJson = request.LocalizationJson;
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
