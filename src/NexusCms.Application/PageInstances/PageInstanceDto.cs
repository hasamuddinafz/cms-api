using NexusCms.Domain.Enums;

namespace NexusCms.Application.PageInstances;

public record PageInstanceDto(
    Guid Id,
    Guid PageModelId,
    string PageModelKey,
    string PageModelName,
    string Slug,
    PageStatus Status,
    string ContentJson,
    string SeoMetaJson,
    long ViewCount,
    DateTime? PublishedAt,
    Guid? CreatedByUserId,
    Guid? UpdatedByUserId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record PageInstanceListItemDto(
    Guid Id,
    Guid PageModelId,
    string PageModelKey,
    string PageModelName,
    string Slug,
    PageStatus Status,
    long ViewCount,
    DateTime? PublishedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record PageInstanceExportRow(
    string PageModelKey,
    string Slug,
    string Status,
    long ViewCount,
    string? PublishedAt,
    string CreatedAt
);
