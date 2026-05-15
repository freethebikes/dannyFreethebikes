using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public sealed class ProjectService : IProjectService
{
    private readonly ISupabaseClientService _client;

    public ProjectService(ISupabaseClientService client)
    {
        _client = client;
    }

    public Task<List<Project>> GetPublicProjectsAsync()
        => _client.GetAllAsync<Project>("projects", "select=*&published=eq.true&order=sort_order.asc");

    public Task<List<Project>> GetAdminProjectsAsync()
        => _client.GetAllAsync<Project>("projects", "select=*&order=sort_order.asc");

    public async Task<Project> SaveProjectAsync(Project project)
    {
        var updated = project with { UpdatedAt = DateTime.UtcNow };
        var result = await _client.UpsertAsync<Project>("projects", updated);
        return result ?? updated;
    }

    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        return await _client.DeleteAsync("projects", id);
    }
}
