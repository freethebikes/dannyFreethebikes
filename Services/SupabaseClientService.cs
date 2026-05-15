using System.Text;
using System.Text.Json;
using FreethebikesSite.Models;
using Microsoft.Extensions.Configuration;

namespace FreethebikesSite.Services;

public sealed class SupabaseClientService : ISupabaseClientService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly Dictionary<string, IList<object>> _fallbackStore;

    public bool IsConfigured { get; }
    public string? SupabaseUrl { get; }
    public string? AnonKey { get; }

    public SupabaseClientService(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        SupabaseUrl = configuration["Supabase:Url"]?.TrimEnd('/');
        AnonKey = configuration["Supabase:AnonKey"];
        IsConfigured = !string.IsNullOrWhiteSpace(SupabaseUrl) && !string.IsNullOrWhiteSpace(AnonKey);

        _fallbackStore = new Dictionary<string, IList<object>>(StringComparer.OrdinalIgnoreCase)
        {
            ["profile"] = new List<object> { SampleData.Profile },
            ["site_settings"] = new List<object> { SampleData.SiteSettings },
            ["projects"] = SampleData.Projects.Cast<object>().ToList(),
            ["photos"] = SampleData.Photos.Cast<object>().ToList(),
            ["skills"] = SampleData.Skills.Cast<object>().ToList(),
            ["notes"] = SampleData.Notes.Cast<object>().ToList(),
            ["links"] = SampleData.Links.Cast<object>().ToList(),
            ["project_images"] = new List<object>()
        };
    }

    public async Task<List<T>> GetAllAsync<T>(string table, string query = "")
    {
        if (!IsConfigured)
        {
            return GetFallbackRecords<T>(table);
        }

        var uri = string.IsNullOrWhiteSpace(query) ? $"{SupabaseUrl}/rest/v1/{table}" : $"{SupabaseUrl}/rest/v1/{table}?{query}";
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        AddAuthHeaders(request);
        request.Headers.Add("Accept", "application/json");

        using var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
    }

    public async Task<T?> GetSingleAsync<T>(string table, string query = "")
    {
        var list = await GetAllAsync<T>(table, query);
        return list.FirstOrDefault();
    }

    public async Task<T?> UpsertAsync<T>(string table, object item)
    {
        if (!IsConfigured)
        {
            return UpsertFallbackRecord<T>(table, item);
        }

        var uri = $"{SupabaseUrl}/rest/v1/{table}";
        var content = new StringContent(JsonSerializer.Serialize(item, _jsonOptions), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = content
        };
        AddAuthHeaders(request);
        request.Headers.Add("Prefer", "return=representation");
        request.Headers.Add("Accept", "application/json");

        using var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var items = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
        return items?.FirstOrDefault();
    }

    public async Task<bool> DeleteAsync(string table, Guid id)
    {
        if (!IsConfigured)
        {
            return DeleteFallbackRecord(table, id);
        }

        var uri = $"{SupabaseUrl}/rest/v1/{table}?id=eq.{id:D}";
        var request = new HttpRequestMessage(HttpMethod.Delete, uri);
        AddAuthHeaders(request);

        using var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<string?> UploadFileAsync(string bucket, Stream contentStream, string fileName, string contentType)
    {
        if (!IsConfigured)
        {
            await Task.CompletedTask;
            return "images/placeholder.svg";
        }

        var uri = $"{SupabaseUrl}/storage/v1/object/{bucket}/{Uri.EscapeDataString(fileName)}";
        using var content = new StreamContent(contentStream);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        var request = new HttpRequestMessage(HttpMethod.Put, uri) { Content = content };
        AddAuthHeaders(request);
        request.Headers.Add("x-upsert", "true");

        using var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return $"{SupabaseUrl}/storage/v1/object/public/{bucket}/{Uri.EscapeDataString(fileName)}";
    }

    private void AddAuthHeaders(HttpRequestMessage request)
    {
        if (!string.IsNullOrWhiteSpace(AnonKey))
        {
            request.Headers.Add("apikey", AnonKey);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AnonKey);
        }
    }

    private List<T> GetFallbackRecords<T>(string table)
    {
        if (_fallbackStore.TryGetValue(table, out var records))
        {
            return records.OfType<T>().ToList();
        }

        return new List<T>();
    }

    private T? UpsertFallbackRecord<T>(string table, object item)
    {
        if (!_fallbackStore.TryGetValue(table, out var records))
        {
            return default;
        }

        switch (table.ToLowerInvariant())
        {
            case "profile":
                records.Clear();
                records.Add(item);
                return (T?)item;
            case "site_settings":
                records.Clear();
                records.Add(item);
                return (T?)item;
            case "projects":
                return SaveFallbackRecord<Project>(records, (Project)item) as T;
            case "photos":
                return SaveFallbackRecord<Photo>(records, (Photo)item) as T;
            case "skills":
                return SaveFallbackRecord<Skill>(records, (Skill)item) as T;
            case "notes":
                return SaveFallbackRecord<Note>(records, (Note)item) as T;
            default:
                return default;
        }
    }

    private T? SaveFallbackRecord<T>(IList<object> records, T item) where T : class
    {
        var typed = records.OfType<T>().ToList();
        var idProperty = item.GetType().GetProperty("Id");
        if (idProperty is null)
        {
            records.Add(item!);
            return item;
        }

        var id = (Guid)idProperty.GetValue(item)!;
        var existing = typed.FirstOrDefault(record => (Guid)record.GetType().GetProperty("Id")!.GetValue(record)! == id);
        if (existing != null)
        {
            records.Remove(existing!);
        }

        records.Add(item!);
        return item;
    }

    private bool DeleteFallbackRecord(string table, Guid id)
    {
        if (!_fallbackStore.TryGetValue(table, out var records))
        {
            return false;
        }

        var typed = records.Where(r => r.GetType().GetProperty("Id") is not null).ToList();
        var existing = typed.FirstOrDefault(record => (Guid)record.GetType().GetProperty("Id")!.GetValue(record)! == id);
        if (existing == null)
        {
            return false;
        }

        records.Remove(existing);
        return true;
    }
}

internal static class SampleData
{
    public static Profile Profile => new()
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
        Links = new List<Link>
        {
            new() { Id = Guid.NewGuid(), Label = "GitHub", Url = "https://github.com/freethebikes", SortOrder = 1, Published = true },
            new() { Id = Guid.NewGuid(), Label = "LinkedIn", Url = "https://linkedin.com/in/freethebikes", SortOrder = 2, Published = true }
        },
        Published = true
    };

    public static SiteSettings SiteSettings => new()
    {
        Id = Guid.NewGuid(),
        HomepageHeadline = "A workshop-style homepage for practical experiments.",
        HomepageSubheadline = "This is a browser-first lab notebook with a dashboard for updates.",
        FeaturedProjectCount = 4,
        ShowFeaturedProjects = true,
        ShowSkills = true,
        ShowFieldNotes = true,
        ShowPhotos = true,
        ShowInterests = true,
        ShowPrinciples = true,
        ShowContact = true,
        StatusText = "status: building",
        ModeText = "mode: field notes",
        SignalText = "signal: practical",
        Published = true
    };

    public static List<Project> Projects => new()
    {
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Surf Forecast Reality Check",
            Slug = "surf-forecast-reality-check",
            ShortDescription = "Collects surf forecasts and observed conditions so I can compare prediction versus reality.",
            LongDescription = "Collects surf forecasts and observed conditions so I can compare what was predicted with what actually happened.",
            Status = "Prototype / active experiment",
            Tags = "Python, Postgres, GitHub Actions, Surf Data, Forecast Analysis",
            Featured = true,
            Published = true,
            SortOrder = 1,
            ProjectUrl = "",
            GitHubUrl = "",
            CoverImage = "images/placeholder.svg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Need a Minute",
            Slug = "need-a-minute",
            ShortDescription = "A calm, offline-first toddler activity app concept for parents who need a short pause.",
            LongDescription = "A calm, offline-first toddler activity app concept for parents who need a short, guilt-free pause.",
            Status = "Concept / prototype planning",
            Tags = "Defold, SQLite, Kids App, Parent Tools, Mobile",
            Featured = true,
            Published = true,
            SortOrder = 2,
            ProjectUrl = "",
            GitHubUrl = "",
            CoverImage = "images/placeholder.svg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Title = "1966 Dodge Travco Systems Log",
            Slug = "1966-dodge-travco-systems-log",
            ShortDescription = "Mechanical, electrical, propane, furnace, fridge, carburetor, and drivetrain troubleshooting on a vintage Dodge Travco.",
            LongDescription = "Mechanical, electrical, propane, furnace, fridge, carburetor, and drivetrain troubleshooting on a vintage Dodge Travco.",
            Status = "Ongoing",
            Tags = "Vintage RV, Electrical, Mechanical, Documentation",
            Featured = true,
            Published = true,
            SortOrder = 3,
            ProjectUrl = "",
            GitHubUrl = "",
            CoverImage = "images/placeholder.svg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Local AI Coding Setup",
            Slug = "local-ai-coding-setup",
            ShortDescription = "Experiments with Codex-like workflows, Ollama, local models, VS Code, Continue, and agentic coding loops.",
            LongDescription = "Experiments with Codex-like workflows, Ollama, local models, VS Code, Continue, and agentic coding loops.",
            Status = "Active learning",
            Tags = "AI Dev Tools, Ollama, Linux, VS Code, Automation",
            Featured = true,
            Published = true,
            SortOrder = 4,
            ProjectUrl = "",
            GitHubUrl = "",
            CoverImage = "images/placeholder.svg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Title = "University/Internal App Work",
            Slug = "university-internal-app-work",
            ShortDescription = "Full-stack internal tools using .NET, Blazor, SQL, authentication, document workflows, and business process automation.",
            LongDescription = "Full-stack internal tools using .NET, Blazor, SQL, authentication, document workflows, and business process automation.",
            Status = "Professional work",
            Tags = ".NET, Blazor, SQL, MudBlazor, Enterprise Apps",
            Featured = true,
            Published = true,
            SortOrder = 5,
            ProjectUrl = "",
            GitHubUrl = "",
            CoverImage = "images/placeholder.svg"
        }
    };

    public static List<Photo> Photos => new()
    {
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Workshop desk",
            Caption = "Current tool stack and next sketch.",
            AltText = "Workbench layout with notebook and laptop.",
            Category = "Workspace",
            SortOrder = 1,
            Published = true,
            ImageUrl = "images/placeholder.svg"
        }
    };

    public static List<Skill> Skills => new()
    {
        new() { Id = Guid.NewGuid(), Name = "C#/.NET", Category = "Software", ProficiencyLabel = "Active", SortOrder = 1, Published = true },
        new() { Id = Guid.NewGuid(), Name = "Postgres", Category = "Data", ProficiencyLabel = "Practical", SortOrder = 2, Published = true },
        new() { Id = Guid.NewGuid(), Name = "Ollama", Category = "AI Tools", ProficiencyLabel = "Experimenting", SortOrder = 3, Published = true },
        new() { Id = Guid.NewGuid(), Name = "Electrical Systems", Category = "Mechanical / Electrical", ProficiencyLabel = "Hands-on", SortOrder = 4, Published = true }
    };

    public static List<Note> Notes => new()
    {
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Forecast vs offshore reality",
            ShortNote = "The forecast held until the last swell period, then the wind shift erased the expected set.",
            Body = "Observed conditions showed that the model predicted the swell window but missed the late-quarter pulse. Next step: log winds separately.",
            Tags = "surf,forecast,field notes",
            Date = DateTime.UtcNow.AddDays(-3),
            SortOrder = 1,
            Published = true
        },
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Battery monitor approach",
            ShortNote = "Use a simple voltage tracker for the old Travco battery bank.",
            Body = "Keep the display in the cab and log values once per day. If voltage drops below 12.1V, check charging subsystem.",
            Tags = "rv,electrical,monitoring",
            Date = DateTime.UtcNow.AddDays(-7),
            SortOrder = 2,
            Published = true
        }
    };

    public static List<Link> Links => new()
    {
        new() { Id = Guid.NewGuid(), Label = "GitHub", Url = "https://github.com/freethebikes", SortOrder = 1, Published = true },
        new() { Id = Guid.NewGuid(), Label = "LinkedIn", Url = "https://linkedin.com/in/freethebikes", SortOrder = 2, Published = true }
    };
}
