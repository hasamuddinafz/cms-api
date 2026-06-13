namespace NexusCms.Application.PageModels;

public record PageModelDto(
    Guid Id,
    string Key,
    string Name,
    string? Description,
    string? Icon,
    bool IsSingleton,
    string SchemaJson,
    int InstanceCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

/// <summary>Flat row used for CSV/Excel export.</summary>
public record PageModelExportRow(
    string Key,
    string Name,
    string? Description,
    bool IsSingleton,
    int InstanceCount,
    DateTime CreatedAt
);
