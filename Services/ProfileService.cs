using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public sealed class ProfileService : IProfileService
{
    private readonly ISupabaseClientService _client;

    public ProfileService(ISupabaseClientService client)
    {
        _client = client;
    }

    public async Task<Profile> GetProfileAsync()
    {
        var profile = await _client.GetSingleAsync<Profile>("profile", "select=*&published=eq.true&limit=1");
        return profile ?? SampleProfile;
    }

    public async Task<Profile> SaveProfileAsync(Profile profile)
    {
        var updated = profile with { UpdatedAt = DateTime.UtcNow };
        var result = await _client.UpsertAsync<Profile>("profile", updated);
        return result ?? updated;
    }

    private static Profile SampleProfile => new()
    {
        Id = Guid.NewGuid(),
        Name = "Danny Freethebikes",
        Handle = "@freethebikes",
        Tagline = "A practical garage lab notebook for half-solved problems.",
        ShortBio = "Some people have a portfolio. I have a pile of half-solved problems, a working theory, and a folder full of notes.",
        HeroIntro = "I build small tools, track experiments, and write the stuff I want to read again.",
        Email = "hello@freethebikes.dev",
        GitHubLink = "https://github.com/freethebikes",
        LinkedInLink = "https://linkedin.com/in/freethebikes",
        Published = true
    };
}
