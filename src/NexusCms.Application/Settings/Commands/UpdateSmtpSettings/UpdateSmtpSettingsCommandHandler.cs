using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Settings.Commands.UpdateSmtpSettings;

public class UpdateSmtpSettingsCommandHandler(IDbContextAccessor db, IUnitOfWork uow, IEncryptionService encryption)
    : IRequestHandler<UpdateSmtpSettingsCommand, Result<AppSettingDto>>
{
    public async Task<Result<AppSettingDto>> Handle(UpdateSmtpSettingsCommand request, CancellationToken cancellationToken)
    {
        var setting = await db.AppSettings.FirstOrDefaultAsync(cancellationToken)
            ?? CreateSingleton(db);

        // If the user left the password unchanged (mask value), preserve the stored encrypted value
        setting.SmtpJson = SmtpJsonHelper.PreserveOrEncrypt(
            request.SmtpJson,
            setting.SmtpJson,
            encryption.Encrypt);

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
