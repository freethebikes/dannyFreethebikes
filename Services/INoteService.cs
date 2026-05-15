using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public interface INoteService
{
    Task<List<Note>> GetPublicNotesAsync();
    Task<List<Note>> GetAdminNotesAsync();
    Task<Note> SaveNoteAsync(Note note);
    Task<bool> DeleteNoteAsync(Guid id);
}
