namespace NotesProject.Notes.models;

public record CreateNoteHandlerRequest
{
  public string Title { get; set; }
  public string Content { get; set; }
}

public record UpdateNoteHandlerRequest : CreateNoteHandlerRequest
{
  public Guid Id { get; set; }
}

public record UpdateNoteHandlerResponse : CreateNoteHandlerResponse
{

}