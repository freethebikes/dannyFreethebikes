namespace FreethebikesSite.Models;

public sealed record Link
{
    public Guid Id { get; init; }
    public string Label { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool Published { get; init; }
}

public sealed record Profile
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public string Name { get; init; } = "Danny Freethebikes";
    public string Handle { get; init; } = "@freethebikes";
    public string Tagline { get; init; } = "A practical garage lab notebook for half-solved problems.";
    public string ShortBio { get; init; } = "Some people have a portfolio. I have a pile of half-solved problems, a working theory, and a folder full of notes.";
    public string HeroIntro { get; init; } = "I build small tools, track experiments, and write the stuff I want to read again.";
    public string Email { get; init; } = "hello@freethebikes.dev";
    public string GitHubLink { get; init; } = "https://github.com/freethebikes";
    public string LinkedInLink { get; init; } = "https://linkedin.com/in/freethebikes";
    public List<Link> Links { get; init; } = new();
    public bool Published { get; init; } = true;
}
