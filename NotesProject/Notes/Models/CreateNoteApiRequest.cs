namespace NotesProject.Notes.models;

public record CreateNoteApiRequest()
{
  public required string Title { get; init; }
  public required string Content  { get; init; }
}

public record UpdateNoteApiRequest: CreateNoteApiRequest
{

}