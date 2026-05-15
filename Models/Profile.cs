namespace FreethebikesSite.Models;

public sealed record Link
{
    public Guid Id { get; init; }
    public string Label { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool Published { get; set; }
}

public sealed record Profile
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = "Danny Freethebikes";
    public string Handle { get; set; } = "@freethebikes";
    public string Tagline { get; set; } = "A practical garage lab notebook for half-solved problems.";
    public string ShortBio { get; set; } = "Some people have a portfolio. I have a pile of half-solved problems, a working theory, and a folder full of notes.";
    public string HeroIntro { get; set; } = "I build small tools, track experiments, and write the stuff I want to read again.";
    public string Email { get; set; } = "hello@freethebikes.dev";
    public string GitHubLink { get; set; } = "https://github.com/freethebikes";
    public string LinkedInLink { get; set; } = "https://linkedin.com/in/freethebikes";
    public List<Link> Links { get; set; } = new();
    public bool Published { get; set; } = true;
}
