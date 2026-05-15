namespace FreethebikesSite.Models;

public sealed record SiteSettings
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public string HomepageHeadline { get; init; } = "A workshop-style homepage for practical experiments.";
    public string HomepageSubheadline { get; init; } = "This is a browser-first lab notebook with a dashboard for updates.";
    public int FeaturedProjectCount { get; init; } = 4;
    public bool ShowFeaturedProjects { get; init; } = true;
    public bool ShowSkills { get; init; } = true;
    public bool ShowFieldNotes { get; init; } = true;
    public bool ShowPhotos { get; init; } = true;
    public bool ShowInterests { get; init; } = true;
    public bool ShowPrinciples { get; init; } = true;
    public bool ShowContact { get; init; } = true;
    public string StatusText { get; init; } = "status: building";
    public string ModeText { get; init; } = "mode: field notes";
    public string SignalText { get; init; } = "signal: practical";
    public bool Published { get; init; } = true;
}
