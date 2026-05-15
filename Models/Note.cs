namespace FreethebikesSite.Models;

public sealed record Note
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public string Title { get; init; } = string.Empty;
    public string ShortNote { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public string Tags { get; init; } = string.Empty;
    public DateTime Date { get; init; } = DateTime.UtcNow;
    public int SortOrder { get; init; }
    public bool Published { get; init; }
}
