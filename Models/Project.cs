namespace FreethebikesSite.Models;

public sealed record Project
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public string LongDescription { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Tags { get; init; } = string.Empty;
    public bool Featured { get; init; }
    public bool Published { get; init; }
    public int SortOrder { get; init; }
    public string ProjectUrl { get; init; } = string.Empty;
    public string GitHubUrl { get; init; } = string.Empty;
    public string CoverImage { get; init; } = "images/placeholder.svg";
    public List<string> GalleryImages { get; init; } = new();
}
