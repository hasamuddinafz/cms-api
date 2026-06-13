using MediatR;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Languages.Commands.DeleteLanguage;

public class DeleteLanguageCommandHandler(IDbContextAccessor db, IUnitOfWork uow)
    : IRequestHandler<DeleteLanguageCommand, Result>
{
    public async Task<Result> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        var language = await db.Languages.FindAsync([request.Id], cancellationToken);
        if (language is null)
            return Result.Failure("Language not found.");

        if (language.IsDefault)
            return Result.Failure("Cannot delete the default language. Set another language as default first.");

        db.Languages.Remove(language);
        await uow.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
