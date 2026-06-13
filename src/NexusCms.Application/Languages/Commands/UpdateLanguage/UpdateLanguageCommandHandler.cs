using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Languages.Commands.UpdateLanguage;

public class UpdateLanguageCommandHandler(IDbContextAccessor db, IUnitOfWork uow)
    : IRequestHandler<UpdateLanguageCommand, Result<LanguageDto>>
{
    public async Task<Result<LanguageDto>> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
    {
        var language = await db.Languages.FindAsync([request.Id], cancellationToken);
        if (language is null)
            return Result<LanguageDto>.Failure("Language not found.");

        if (request.IsDefault)
            await db.Languages
                .Where(l => l.IsDefault && l.Id != request.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.IsDefault, false), cancellationToken);

        language.Name      = request.Name;
        language.IsDefault = request.IsDefault;
        language.IsActive  = request.IsActive;
        language.FlagIcon  = request.FlagIcon;
        language.SortOrder = request.SortOrder;

        await uow.SaveChangesAsync(cancellationToken);

        return Result<LanguageDto>.Success(new LanguageDto(
            language.Id, language.Code, language.Name,
            language.IsDefault, language.IsActive, language.FlagIcon, language.SortOrder));
    }
}
