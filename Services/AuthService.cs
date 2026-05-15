using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace FreethebikesSite.Services;

public sealed class AuthService : IAuthService
{
    private const string TokenKey = "supabase_access_token";
    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;
    private readonly IJSRuntime _jsRuntime;
    private readonly SupabaseAuthenticationStateProvider _stateProvider;

    public bool IsAuthenticated { get; private set; }
    public string? CurrentEmail { get; private set; }

    public AuthService(HttpClient http, IConfiguration configuration, IJSRuntime jsRuntime, AuthenticationStateProvider stateProvider)
    {
        _http = http;
        _configuration = configuration;
        _jsRuntime = jsRuntime;
        _stateProvider = (SupabaseAuthenticationStateProvider)stateProvider;
    }

    public async Task InitializeAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        if (!string.IsNullOrWhiteSpace(token))
        {
            IsAuthenticated = true;
            CurrentEmail = null;
            _stateProvider.NotifyAuthenticationStateChanged();
        }
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var supabaseUrl = _configuration["Supabase:Url"]?.TrimEnd('/');
        var anonKey = _configuration["Supabase:AnonKey"];
        if (string.IsNullOrWhiteSpace(supabaseUrl) || string.IsNullOrWhiteSpace(anonKey))
        {
            return false;
        }

        var requestUri = $"{supabaseUrl}/auth/v1/token?grant_type=password";
        var payload = new { email, password };
        var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };
        request.Headers.Add("apikey", anonKey);
        request.Headers.Add("Accept", "application/json");

        using var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        if (!document.RootElement.TryGetProperty("access_token", out var tokenElement))
        {
            return false;
        }

        var accessToken = tokenElement.GetString();
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return false;
        }

        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, accessToken);
        IsAuthenticated = true;
        CurrentEmail = email;
        _stateProvider.NotifyAuthenticationStateChanged();
        return true;
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        IsAuthenticated = false;
        CurrentEmail = null;
        _stateProvider.NotifyAuthenticationStateChanged();
    }
}

public sealed class SupabaseAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService _authService;

    public SupabaseAuthenticationStateProvider(IAuthService authService)
    {
        _authService = authService;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsIdentity identity = _authService.IsAuthenticated
            ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, _authService.CurrentEmail ?? "admin") }, "supabase")
            : new ClaimsIdentity();

        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
