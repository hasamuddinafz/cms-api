using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Public.Queries.GetPublicLanguages;

public class GetPublicLanguagesQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetPublicLanguagesQuery, Result<List<PublicLanguageDto>>>
{
    public async Task<Result<List<PublicLanguageDto>>> Handle(GetPublicLanguagesQuery request, CancellationToken ct)
    {
        var languages = await db.Languages
            .Where(l => l.IsActive)
            .OrderBy(l => l.SortOrder)
            .ThenBy(l => l.Name)
            .Select(l => new PublicLanguageDto(l.Code, l.Name, l.IsDefault, l.FlagIcon ?? "", l.SortOrder))
            .ToListAsync(ct);

        return Result<List<PublicLanguageDto>>.Success(languages);
    }
}
