using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Settings.Commands.UpdateSeoSettings;

public class UpdateSeoSettingsCommandHandler(IDbContextAccessor db, IUnitOfWork uow)
    : IRequestHandler<UpdateSeoSettingsCommand, Result<AppSettingDto>>
{
    public async Task<Result<AppSettingDto>> Handle(UpdateSeoSettingsCommand request, CancellationToken cancellationToken)
    {
        var setting = await db.AppSettings.FirstOrDefaultAsync(cancellationToken)
            ?? CreateSingleton(db);

        setting.SeoJson = request.SeoJson;
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
