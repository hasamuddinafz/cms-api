namespace NexusCms.Application.Dashboard;

public record DashboardStatsDto(
    int  TotalPages,
    int  PublishedPages,
    long TotalViews,
    long TodayViews,
    int  TotalAssets,
    int  TotalUsers);

public record DailyViewDto(string Date, long Views);

public record TopPageDto(
    Guid   Id,
    string Slug,
    string ModelKey,
    long   ViewCount);

public record RecentActivityDto(
    Guid      Id,
    string    Action,
    string    EntityType,
    string?   EntityId,
    string?   UserEmail,
    DateTime  CreatedAt);
