namespace NexusCms.Application.Common.Models;

public class PagedRequest
{
    public string? Search { get; init; }
    public string? SortBy { get; init; }
    public string SortDir { get; init; } = "asc";  // "asc" | "desc"
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;

    // Generic key-value filters (e.g. { "status": "Published", "type": "Image" })
    public Dictionary<string, string> Filters { get; init; } = [];

    public int Skip => (Page - 1) * PageSize;
}
