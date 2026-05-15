namespace FreethebikesSite.Models;

public sealed record Skill
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public string Name { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string ProficiencyLabel { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool Published { get; init; }
}
