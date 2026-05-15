using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public sealed class SiteSettingsService : ISiteSettingsService
{
    private readonly ISupabaseClientService _client;

    public SiteSettingsService(ISupabaseClientService client)
    {
        _client = client;
    }

    public async Task<SiteSettings> GetSettingsAsync()
    {
        var settings = await _client.GetSingleAsync<SiteSettings>("site_settings", "select=*&published=eq.true&limit=1");
        return settings ?? SampleSettings;
    }

    public async Task<SiteSettings> SaveSettingsAsync(SiteSettings settings)
    {
        var updated = settings with { UpdatedAt = DateTime.UtcNow };
        var result = await _client.UpsertAsync<SiteSettings>("site_settings", updated);
        return result ?? updated;
    }

    private static SiteSettings SampleSettings => new()
    {
        Id = Guid.NewGuid(),
        HomepageHeadline = "A workshop-style homepage for practical experiments.",
        HomepageSubheadline = "This is a browser-first lab notebook with a dashboard for updates.",
        FeaturedProjectCount = 4,
        ShowFeaturedProjects = true,
        ShowSkills = true,
        ShowFieldNotes = true,
        ShowPhotos = true,
        ShowInterests = true,
        ShowPrinciples = true,
        ShowContact = true,
        StatusText = "status: building",
        ModeText = "mode: field notes",
        SignalText = "signal: practical",
        Published = true
    };
}
