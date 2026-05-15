using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public interface IProfileService
{
    Task<Profile> GetProfileAsync();
    Task<Profile> SaveProfileAsync(Profile profile);
}
