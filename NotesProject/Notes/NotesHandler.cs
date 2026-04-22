using NotesProject.Notes.Entities;
using NotesProject.Notes.models;
using NotesProject.Notes.Models;

namespace NotesProject.Notes;

public interface INotesHandler
{
  Task<CreateNoteHandlerResponse> CreateNote(CreateNoteHandlerRequest  request, CancellationToken cancellationToken);
  Task<IEnumerable<NoteDto>> GetNotes(CancellationToken cancellationToken);

  /// <summary>
  /// Updates a given note entity. Returns null if the note does not exist.
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<UpdateNoteHandlerResponse?> UpdateNote(UpdateNoteHandlerRequest request, CancellationToken cancellationToken);

  /// <summary>
  /// Deletes the note in the data storage, returns null if the note does not exist.
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<NoteDto?> DeleteNote(Guid noteId, CancellationToken cancellationToken);
}

public class NotesHandler : INotesHandler
{
  private List<Note> _notes;

  public NotesHandler()
  {
    _notes = new List<Note>();
  }

  public Task<CreateNoteHandlerResponse> CreateNote(CreateNoteHandlerRequest request, CancellationToken cancellationToken)
  {
    var newNote = new Note()
    {
      Id = Guid.NewGuid(),
      Title = request.Title,
      Content = request.Content,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };
    _notes.Add(newNote);
    return Task.FromResult(new CreateNoteHandlerResponse()
    {
      Id = newNote.Id,
      Title = newNote.Title,
      Content = newNote.Content,
      CreatedAt = newNote.CreatedAt,
      UpdatedAt = newNote.UpdatedAt
    });
  }

  public Task<IEnumerable<NoteDto>> GetNotes(CancellationToken cancellationToken)
  {
    return Task.FromResult(_notes.Select(note => new NoteDto()
    {
      Id = note.Id,
      Title = note.Title,
      Content = note.Content,
      CreatedAt = note.CreatedAt,
      UpdatedAt = note.UpdatedAt
    }));
  }

  public async Task<UpdateNoteHandlerResponse?> UpdateNote(UpdateNoteHandlerRequest request, CancellationToken cancellationToken)
  {
    // find the note
    var note = await GetNote(request.Id, cancellationToken);
    if (note == null)
    {
      return null;
    }
    // update the note fields
    note.Title = request.Title;
    note.Content = request.Content;
    note.UpdatedAt = DateTime.UtcNow;
    return new UpdateNoteHandlerResponse()
    {
      Id = note.Id,
      Title = note.Title,
      Content = note.Content,
      CreatedAt = note.CreatedAt,
      UpdatedAt = note.UpdatedAt
    };
  }

  public async Task<NoteDto?> DeleteNote(Guid noteId, CancellationToken cancellationToken)
  {
    // find note's index
    var noteIdx = _notes.FindIndex(note => note.Id == noteId);
    if (noteIdx == -1)
    {
      return null;
    }
    _notes.RemoveAt(noteIdx);
    return new NoteDto()
    {
    };
  }

  private Task<Note?> GetNote(Guid noteId, CancellationToken cancellationToken)
  {
    return Task.FromResult(_notes.FirstOrDefault(note => note.Id == noteId));
  }
}