using DataPlatform.Infrastructure.Entities;

namespace DataPlatform.Infrastructure.Interfaces
{
    public interface ILocalizationRepository
    {
        Task<IEnumerable<LocalizationEntity>> GetByLanguageAsync(string language);
        Task<LocalizationEntity> CreateOrUpdateAsync(LocalizationEntity localization);
        Task AddRangeAsync(IEnumerable<LocalizationEntity> localizations);
    }
}