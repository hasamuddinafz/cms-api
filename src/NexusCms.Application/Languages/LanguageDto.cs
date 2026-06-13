namespace NexusCms.Application.Languages;

public record LanguageDto(
    Guid   Id,
    string Code,
    string Name,
    bool   IsDefault,
    bool   IsActive,
    string FlagIcon,
    int    SortOrder);
