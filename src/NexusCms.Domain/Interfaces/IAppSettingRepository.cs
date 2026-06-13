using NexusCms.Domain.Entities;

namespace NexusCms.Domain.Interfaces;

public interface IAppSettingRepository
{
    Task<AppSetting> GetAsync(CancellationToken ct = default);
    Task UpdateAsync(AppSetting setting, CancellationToken ct = default);
}
