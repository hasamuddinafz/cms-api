using MediatR;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.Assets.Commands.UpdateAltText;

public class UpdateAltTextCommandHandler(IDbContextAccessor db)
    : IRequestHandler<UpdateAltTextCommand, Result>
{
    public async Task<Result> Handle(UpdateAltTextCommand request, CancellationToken ct)
    {
        var asset = await db.FileAssets.FindAsync([request.Id], ct);
        if (asset is null)
            return Result.Failure("Dosya bulunamadı.");

        if (asset.FileType != FileType.Image)
            return Result.Failure("Alt metin yalnızca görsel (Image) tipindeki dosyalar için güncellenebilir.");

        asset.AltTextJson = request.AltTextJson;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
