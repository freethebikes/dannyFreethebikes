namespace FreethebikesSite.Models;

public sealed record Project
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public bool Featured { get; set; }
    public bool Published { get; set; }
    public int SortOrder { get; set; }
    public string ProjectUrl { get; set; } = string.Empty;
    public string GitHubUrl { get; set; } = string.Empty;
    public string CoverImage { get; set; } = "images/placeholder.svg";
    public List<string> GalleryImages { get; set; } = new();
}
