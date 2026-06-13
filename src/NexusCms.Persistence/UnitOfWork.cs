using NexusCms.Domain.Interfaces;

namespace NexusCms.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);

    public void Dispose() => context.Dispose();
}
