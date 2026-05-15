namespace FreethebikesSite.Models;

public sealed record Photo
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Title { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Guid? RelatedProjectId { get; set; }
    public int SortOrder { get; set; }
    public bool Published { get; set; }
    public string ImageUrl { get; set; } = "images/placeholder.svg";
}
