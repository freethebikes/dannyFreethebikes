using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public sealed class SkillService : ISkillService
{
    private readonly ISupabaseClientService _client;

    public SkillService(ISupabaseClientService client)
    {
        _client = client;
    }

    public Task<List<Skill>> GetPublicSkillsAsync()
        => _client.GetAllAsync<Skill>("skills", "select=*&published=eq.true&order=sort_order.asc");

    public Task<List<Skill>> GetAdminSkillsAsync()
        => _client.GetAllAsync<Skill>("skills", "select=*&order=sort_order.asc");

    public async Task<Skill> SaveSkillAsync(Skill skill)
    {
        var updated = skill with { UpdatedAt = DateTime.UtcNow };
        var result = await _client.UpsertAsync<Skill>("skills", updated);
        return result ?? updated;
    }

    public async Task<bool> DeleteSkillAsync(Guid id)
    {
        return await _client.DeleteAsync("skills", id);
    }
}
