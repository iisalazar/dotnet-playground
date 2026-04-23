using Microsoft.EntityFrameworkCore;
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
  private readonly NotesProjectContext _context;

  public NotesHandler(NotesProjectContext context)
  {
    _context = context;
  }

  public async Task<CreateNoteHandlerResponse> CreateNote(CreateNoteHandlerRequest request, CancellationToken cancellationToken)
  {
    var newNote = new Note()
    {
      Title = request.Title,
      Content = request.Content,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };
    _context.Notes.Add(newNote);
    await _context.SaveChangesAsync(cancellationToken);
    return new CreateNoteHandlerResponse()
    {
      Id = newNote.Id,
      Title = newNote.Title,
      Content = newNote.Content,
      CreatedAt = newNote.CreatedAt,
      UpdatedAt = newNote.UpdatedAt
    };
  }

  public async Task<IEnumerable<NoteDto>> GetNotes(CancellationToken cancellationToken)
  {
    // TODO Add pagination and filters
    var notes = await _context.Notes
      .Select(note => new NoteDto()
      {
        Id = note.Id,
        Title = note.Title,
        Content = note.Content,
        CreatedAt = note.CreatedAt,
        UpdatedAt = note.UpdatedAt
      })
      .ToArrayAsync(cancellationToken);
    return notes;
  }

  public async Task<UpdateNoteHandlerResponse?> UpdateNote(UpdateNoteHandlerRequest request, CancellationToken cancellationToken)
  {
    // find the note
    var note = await GetMutableNote(request.Id, cancellationToken);
    if (note == null)
    {
      return null;
    }
    // update the note fields
    note.Title = request.Title;
    note.Content = request.Content;
    note.UpdatedAt = DateTime.UtcNow;
    await _context.SaveChangesAsync(cancellationToken);
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
    var note = await GetMutableNote(noteId, cancellationToken);
    if (note == null)
    {
      return null;
    }
    _context.Notes.Remove(note);
    await _context.SaveChangesAsync(cancellationToken);
    return new NoteDto()
    {
      Id = note.Id,
      Title = note.Title,
      Content = note.Content,
      CreatedAt = note.CreatedAt,
      UpdatedAt = note.UpdatedAt
    };
  }

  private async Task<Note?> GetMutableNote(Guid noteId, CancellationToken cancellationToken)
  {
    var note = await _context.Notes.FirstOrDefaultAsync(x => x.Id.Equals(noteId), cancellationToken);
    return note;
  }
}