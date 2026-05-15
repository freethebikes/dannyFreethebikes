using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public interface ISkillService
{
    Task<List<Skill>> GetPublicSkillsAsync();
    Task<List<Skill>> GetAdminSkillsAsync();
    Task<Skill> SaveSkillAsync(Skill skill);
    Task<bool> DeleteSkillAsync(Guid id);
}
