using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public interface IPhotoService
{
    Task<List<Photo>> GetPublicPhotosAsync();
    Task<List<Photo>> GetAdminPhotosAsync();
    Task<Photo> SavePhotoAsync(Photo photo);
    Task<bool> DeletePhotoAsync(Guid id);
    Task<string?> UploadPhotoAsync(Stream photoStream, string fileName, string contentType);
}
