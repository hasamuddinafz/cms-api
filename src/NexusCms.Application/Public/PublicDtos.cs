using NexusCms.Application.Common.Models;

namespace NexusCms.Application.Public;

/// <summary>Single published page returned to the frontend.</summary>
public record PublicPageDto(
    Guid   Id,
    string ModelKey,
    string Slug,
    string ContentJson,
    string SeoMetaJson,
    long   ViewCount,
    DateTime? PublishedAt);

/// <summary>List item for paginated page lists (e.g. blog index).</summary>
public record PublicPageListItemDto(
    Guid   Id,
    string Slug,
    string SeoMetaJson,
    long   ViewCount,
    DateTime? PublishedAt);

/// <summary>Public settings (no SMTP, no encryption-sensitive data).</summary>
public record PublicSettingsDto(
    string GeneralJson,
    string SeoJson,
    string LocalizationJson,
    string ThemeJson);

/// <summary>Active language record for the frontend.</summary>
public record PublicLanguageDto(
    string Code,
    string Name,
    bool   IsDefault,
    string FlagIcon,
    int    SortOrder);

/// <summary>Menu tree returned to the frontend.</summary>
public record PublicMenuDto(
    string Key,
    string ItemsJson);
