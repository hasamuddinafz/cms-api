using NexusCms.Domain.Entities;

namespace NexusCms.Domain.Interfaces;

public interface IPageModelRepository : IRepository<PageModel>
{
    Task<PageModel?> GetByKeyAsync(string key, CancellationToken ct = default);
    Task<bool> KeyExistsAsync(string key, CancellationToken ct = default);
}
