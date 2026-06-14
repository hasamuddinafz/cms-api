namespace NexusCms.Application.MailTemplates;

public record MailTemplateDto(
    Guid Id,
    string Name,
    string Subject,
    string HtmlBody,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record MailTemplateListItemDto(
    Guid Id,
    string Name,
    string Subject,
    DateTime CreatedAt);
