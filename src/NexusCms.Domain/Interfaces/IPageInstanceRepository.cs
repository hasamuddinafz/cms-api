using NexusCms.Domain.Entities;
using NexusCms.Domain.Enums;

namespace NexusCms.Domain.Interfaces;

public interface IPageInstanceRepository : IRepository<PageInstance>
{
    Task<PageInstance?> GetBySlugAsync(string modelKey, string slug, CancellationToken ct = default);
    Task IncrementViewCountAsync(Guid id, CancellationToken ct = default);
}
