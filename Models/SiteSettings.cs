namespace FreethebikesSite.Models;

public sealed record SiteSettings
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string HomepageHeadline { get; set; } = "A workshop-style homepage for practical experiments.";
    public string HomepageSubheadline { get; set; } = "This is a browser-first lab notebook with a dashboard for updates.";
    public int FeaturedProjectCount { get; set; } = 4;
    public bool ShowFeaturedProjects { get; set; } = true;
    public bool ShowSkills { get; set; } = true;
    public bool ShowFieldNotes { get; set; } = true;
    public bool ShowPhotos { get; set; } = true;
    public bool ShowInterests { get; set; } = true;
    public bool ShowPrinciples { get; set; } = true;
    public bool ShowContact { get; set; } = true;
    public string StatusText { get; set; } = "status: building";
    public string ModeText { get; set; } = "mode: field notes";
    public string SignalText { get; set; } = "signal: practical";
    public bool Published { get; set; } = true;
}
