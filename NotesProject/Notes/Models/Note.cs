namespace NotesProject.Notes.Models;

public record NoteDto
{
  public Guid Id { get; init; }
  public string Title { get; init; }
  public string Content { get; init; }
  public DateTime CreatedAt { get; init; }
  public DateTime UpdatedAt { get; init; }
}