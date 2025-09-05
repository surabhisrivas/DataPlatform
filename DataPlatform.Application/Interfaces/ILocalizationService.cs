using DataPlatform.Shared.DTOs;

namespace DataPlatform.Application.Interfaces
{
    public interface ILocalizationService
    {
        Task<Dictionary<string, string>> GetLocalizationsAsync(string language);
        Task<LocalizationDto> CreateOrUpdateAsync(CreateLocalizationDto dto);
        Task AddMissingLocalizationsAsync(string language, Dictionary<string, string> translations);
    }
}