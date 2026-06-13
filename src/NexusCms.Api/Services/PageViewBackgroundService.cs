using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Entities;
using NexusCms.Persistence;

namespace NexusCms.Api.Services;

/// <summary>
/// Implements both <see cref="IPageViewQueue"/> (enqueue side, used by the request pipeline)
/// and <see cref="BackgroundService"/> (dequeue side, runs on a dedicated thread).
/// Uses a bounded Channel so back-pressure is applied if the DB writer falls behind.
/// </summary>
public class PageViewBackgroundService : BackgroundService, IPageViewQueue
{
    private readonly Channel<PageViewLogEntry> _channel =
        Channel.CreateBounded<PageViewLogEntry>(new BoundedChannelOptions(5_000)
        {
            FullMode        = BoundedChannelFullMode.DropOldest,
            SingleReader    = true,
            SingleWriter    = false
        });

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PageViewBackgroundService> _logger;

    public PageViewBackgroundService(IServiceScopeFactory scopeFactory, ILogger<PageViewBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger       = logger;
    }

    /// <summary>Called from the request pipeline — must be non-blocking.</summary>
    public void Enqueue(PageViewLogEntry entry) =>
        _channel.Writer.TryWrite(entry);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var entry in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await WriteLogAsync(entry, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "PageViewLog write failed for PageInstance {Id}", entry.PageInstanceId);
            }
        }
    }

    private async Task WriteLogAsync(PageViewLogEntry entry, CancellationToken ct)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.PageViewLogs.Add(new PageViewLog
        {
            PageInstanceId = entry.PageInstanceId,
            ViewedAt       = DateTime.UtcNow,
            IpHash         = Hash(entry.IpHash),
            UserAgent      = entry.UserAgent,
            Referrer       = entry.Referrer
        });

        await db.SaveChangesAsync(ct);
    }

    private static string Hash(string ip)
    {
        if (string.IsNullOrEmpty(ip)) return string.Empty;
        var bytes = System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(ip));
        return Convert.ToHexString(bytes)[..16]; // first 8 bytes as hex
    }
}
