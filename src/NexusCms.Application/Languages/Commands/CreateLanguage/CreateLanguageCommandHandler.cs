using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Languages.Commands.CreateLanguage;

public class CreateLanguageCommandHandler(IDbContextAccessor db, IUnitOfWork uow)
    : IRequestHandler<CreateLanguageCommand, Result<LanguageDto>>
{
    public async Task<Result<LanguageDto>> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        if (await db.Languages.AnyAsync(l => l.Code == request.Code, cancellationToken))
            return Result<LanguageDto>.Failure($"A language with code '{request.Code}' already exists.");

        // If this one is marked default, unset others
        if (request.IsDefault)
            await db.Languages
                .Where(l => l.IsDefault)
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.IsDefault, false), cancellationToken);

        var language = new Language
        {
            Code      = request.Code.ToLowerInvariant(),
            Name      = request.Name,
            IsDefault = request.IsDefault,
            IsActive  = request.IsActive,
            FlagIcon  = request.FlagIcon,
            SortOrder = request.SortOrder
        };

        db.Languages.Add(language);
        await uow.SaveChangesAsync(cancellationToken);

        return Result<LanguageDto>.Success(ToDto(language));
    }

    internal static LanguageDto ToDto(Language l) =>
        new(l.Id, l.Code, l.Name, l.IsDefault, l.IsActive, l.FlagIcon, l.SortOrder);
}
