using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public interface ISiteSettingsService
{
    Task<SiteSettings> GetSettingsAsync();
    Task<SiteSettings> SaveSettingsAsync(SiteSettings settings);
}
