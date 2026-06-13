namespace NexusCms.Application.Common.Interfaces;

/// <summary>
/// Lightweight entry queued for async PageViewLog insertion.
/// </summary>
public record PageViewLogEntry(
    Guid   PageInstanceId,
    string IpHash,
    string UserAgent,
    string Referrer);

/// <summary>
/// Decouples the request pipeline from the PageViewLog writer.
/// Implemented by PageViewBackgroundService in the Api project.
/// </summary>
public interface IPageViewQueue
{
    void Enqueue(PageViewLogEntry entry);
}
