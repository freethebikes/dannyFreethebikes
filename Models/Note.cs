namespace FreethebikesSite.Models;

public sealed record Note
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Title { get; set; } = string.Empty;
    public string ShortNote { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int SortOrder { get; set; }
    public bool Published { get; set; }
}
