using Microsoft.AspNetCore.Mvc;
using NotesProject.Notes.models;
using NotesProject.Notes.Models;

namespace NotesProject.Notes;

[ApiController]
[Route("notes")]
public class NotesController : ControllerBase
{
    private readonly ILogger<NotesController> _logger;
    private readonly INotesHandler _notesHandler;

    public NotesController(ILogger<NotesController> logger, INotesHandler notesHandler)
    {
        _logger = logger;
        _notesHandler = notesHandler;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NoteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var response = await _notesHandler.GetNotes(cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(int id)
    {
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(NoteDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateNoteApiRequest request, CancellationToken cancellationToken)
    {
        var created = await _notesHandler.CreateNote(new CreateNoteHandlerRequest()
        {
            Content = request.Content,
            Title = request.Title,
        }, cancellationToken);
        return Created("", created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(NoteDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNoteApiRequest request, CancellationToken cancellationToken)
    {
        var updated = await _notesHandler.UpdateNote(new UpdateNoteHandlerRequest()
        {
            Id = id,
            Content = request.Content,
            Title = request.Title,
        },  cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _notesHandler.DeleteNote(id, cancellationToken);
        return Ok(deleted);
    }
}