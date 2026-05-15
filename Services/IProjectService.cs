using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public interface IProjectService
{
    Task<List<Project>> GetPublicProjectsAsync();
    Task<List<Project>> GetAdminProjectsAsync();
    Task<Project> SaveProjectAsync(Project project);
    Task<bool> DeleteProjectAsync(Guid id);
}
