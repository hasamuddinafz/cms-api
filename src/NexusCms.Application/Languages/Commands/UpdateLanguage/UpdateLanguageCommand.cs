using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Languages.Commands.UpdateLanguage;

public record UpdateLanguageCommand(
    Guid   Id,
    string Name,
    bool   IsDefault,
    bool   IsActive,
    string FlagIcon,
    int    SortOrder) : IRequest<Result<LanguageDto>>;
