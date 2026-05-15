using System.IO;

namespace FreethebikesSite.Services;

public interface ISupabaseClientService
{
    bool IsConfigured { get; }
    string? SupabaseUrl { get; }
    string? AnonKey { get; }
    Task<List<T>> GetAllAsync<T>(string table, string query = "");
    Task<T?> GetSingleAsync<T>(string table, string query = "");
    Task<T?> UpsertAsync<T>(string table, object item);
    Task<bool> DeleteAsync(string table, Guid id);
    Task<string?> UploadFileAsync(string bucket, Stream content, string fileName, string contentType);
}
