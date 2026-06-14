namespace NexusCms.Application.Common.Models;

public class PagedRequest
{
    private int _page = 1;
    private int _pageSize = 20;

    public string? Search { get; init; }
    public string? SortBy { get; init; }
    public string SortDir { get; init; } = "asc";  // "asc" | "desc"

    public int Page
    {
        get => _page;
        init => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value < 1 ? 1 : value > 100 ? 100 : value;
    }

    // Generic key-value filters (e.g. { "status": "Published", "type": "Image" })
    public Dictionary<string, string> Filters { get; init; } = [];

    public int Skip => (Page - 1) * PageSize;
}
