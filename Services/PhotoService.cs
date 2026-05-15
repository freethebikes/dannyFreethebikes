using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public sealed class PhotoService : IPhotoService
{
    private readonly ISupabaseClientService _client;

    public PhotoService(ISupabaseClientService client)
    {
        _client = client;
    }

    public Task<List<Photo>> GetPublicPhotosAsync()
        => _client.GetAllAsync<Photo>("photos", "select=*&published=eq.true&order=sort_order.asc");

    public Task<List<Photo>> GetAdminPhotosAsync()
        => _client.GetAllAsync<Photo>("photos", "select=*&order=sort_order.asc");

    public async Task<Photo> SavePhotoAsync(Photo photo)
    {
        var updated = photo with { UpdatedAt = DateTime.UtcNow };
        var result = await _client.UpsertAsync<Photo>("photos", updated);
        return result ?? updated;
    }

    public async Task<bool> DeletePhotoAsync(Guid id)
    {
        return await _client.DeleteAsync("photos", id);
    }

    public Task<string?> UploadPhotoAsync(Stream photoStream, string fileName, string contentType)
    {
        return _client.UploadFileAsync("portfolio-assets", photoStream, fileName, contentType);
    }
}
