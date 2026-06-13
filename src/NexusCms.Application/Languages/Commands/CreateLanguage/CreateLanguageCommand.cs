using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Languages.Commands.CreateLanguage;

public record CreateLanguageCommand(
    string Code,
    string Name,
    bool   IsDefault,
    bool   IsActive,
    string FlagIcon,
    int    SortOrder) : IRequest<Result<LanguageDto>>;
