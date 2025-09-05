using DataPlatform.Infrastructure.Interfaces;
using DataPlatform.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataPlatform.Infrastructure.Repositories
{
    public class LocalizationRepository : ILocalizationRepository
    {
        private readonly AppDbContext _context;

        public LocalizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocalizationEntity>> GetByLanguageAsync(string language)
        {
            return await _context.Localizations
                .Where(l => l.Language == language)
                .ToListAsync();
        }

        public async Task<LocalizationEntity> CreateOrUpdateAsync(LocalizationEntity localization)
        {
            var existing = await _context.Localizations
                .FirstOrDefaultAsync(l => l.Key == localization.Key && l.Language == localization.Language);

            if (existing != null)
            {
                existing.Value = localization.Value;
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existing;
            }
            else
            {
                _context.Localizations.Add(localization);
                await _context.SaveChangesAsync();
                return localization;
            }
        }

        public async Task AddRangeAsync(IEnumerable<LocalizationEntity> localizations)
        {
            var existingKeys = await _context.Localizations
                .Where(l => localizations.Any(newL => newL.Key == l.Key && newL.Language == l.Language))
                .Select(l => new { l.Key, l.Language })
                .ToListAsync();

            var newLocalizations = localizations.Where(l => 
                !existingKeys.Any(ek => ek.Key == l.Key && ek.Language == l.Language));

            if (newLocalizations.Any())
            {
                _context.Localizations.AddRange(newLocalizations);
                await _context.SaveChangesAsync();
            }
        }
    }
}