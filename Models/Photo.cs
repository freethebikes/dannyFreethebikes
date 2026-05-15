namespace FreethebikesSite.Models;

public sealed record Photo
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public string Title { get; init; } = string.Empty;
    public string Caption { get; init; } = string.Empty;
    public string AltText { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public Guid? RelatedProjectId { get; init; }
    public int SortOrder { get; init; }
    public bool Published { get; init; }
    public string ImageUrl { get; init; } = "images/placeholder.svg";
}
