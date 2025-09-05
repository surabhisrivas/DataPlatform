using DataPlatform.Application.Interfaces;
using DataPlatform.Infrastructure.Entities;
using DataPlatform.Infrastructure.Interfaces;
using DataPlatform.Shared.DTOs;

namespace DataPlatform.Application.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ILocalizationRepository _localizationRepository;

        public LocalizationService(ILocalizationRepository localizationRepository)
        {
            _localizationRepository = localizationRepository;
        }

        public async Task<Dictionary<string, string>> GetLocalizationsAsync(string language)
        {
            var localizations = await _localizationRepository.GetByLanguageAsync(language);
            return localizations.ToDictionary(l => l.Key, l => l.Value);
        }

        public async Task<LocalizationDto> CreateOrUpdateAsync(CreateLocalizationDto dto)
        {
            var localization = new LocalizationEntity
            {
                Key = dto.Key,
                Language = dto.Language,
                Value = dto.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _localizationRepository.CreateOrUpdateAsync(localization);
            
            return new LocalizationDto
            {
                Id = result.Id,
                Key = result.Key,
                Language = result.Language,
                Value = result.Value
            };
        }

        public async Task AddMissingLocalizationsAsync(string language, Dictionary<string, string> translations)
        {
            var localizations = translations.Select(kvp => new LocalizationEntity
            {
                Key = kvp.Key,
                Language = language,
                Value = kvp.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _localizationRepository.AddRangeAsync(localizations);
        }
    }
}