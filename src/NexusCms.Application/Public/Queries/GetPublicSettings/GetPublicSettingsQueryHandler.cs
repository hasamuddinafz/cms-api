using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Public.Queries.GetPublicSettings;

public class GetPublicSettingsQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetPublicSettingsQuery, Result<PublicSettingsDto>>
{
    public async Task<Result<PublicSettingsDto>> Handle(GetPublicSettingsQuery request, CancellationToken ct)
    {
        var setting = await db.AppSettings.FirstOrDefaultAsync(ct);

        // Return sensible empty defaults if singleton hasn't been saved yet
        return Result<PublicSettingsDto>.Success(new PublicSettingsDto(
            setting?.GeneralJson      ?? "{}",
            setting?.SeoJson          ?? "{}",
            setting?.LocalizationJson ?? "{}",
            setting?.ThemeJson        ?? "{}"));
    }
}
