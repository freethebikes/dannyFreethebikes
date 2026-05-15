namespace FreethebikesSite.Services;

public interface IAuthService
{
    bool IsAuthenticated { get; }
    string? CurrentEmail { get; }
    Task InitializeAsync();
    Task<bool> LoginAsync(string email, string password);
    Task LogoutAsync();
}
