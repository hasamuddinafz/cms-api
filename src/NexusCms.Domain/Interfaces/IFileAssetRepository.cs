using NexusCms.Domain.Entities;
using NexusCms.Domain.Enums;

namespace NexusCms.Domain.Interfaces;

public interface IFileAssetRepository : IRepository<FileAsset>
{
    Task<IReadOnlyList<FileAsset>> GetByTypeAsync(FileType fileType, CancellationToken ct = default);
}
