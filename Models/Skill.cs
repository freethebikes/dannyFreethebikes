namespace FreethebikesSite.Models;

public sealed record Skill
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ProficiencyLabel { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool Published { get; set; }
}
