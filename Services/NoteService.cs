using FreethebikesSite.Models;

namespace FreethebikesSite.Services;

public sealed class NoteService : INoteService
{
    private readonly ISupabaseClientService _client;

    public NoteService(ISupabaseClientService client)
    {
        _client = client;
    }

    public Task<List<Note>> GetPublicNotesAsync()
        => _client.GetAllAsync<Note>("notes", "select=*&published=eq.true&order=date.desc");

    public Task<List<Note>> GetAdminNotesAsync()
        => _client.GetAllAsync<Note>("notes", "select=*&order=date.desc");

    public async Task<Note> SaveNoteAsync(Note note)
    {
        var updated = note with { UpdatedAt = DateTime.UtcNow };
        var result = await _client.UpsertAsync<Note>("notes", updated);
        return result ?? updated;
    }

    public async Task<bool> DeleteNoteAsync(Guid id)
    {
        return await _client.DeleteAsync("notes", id);
    }
}
