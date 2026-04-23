using Microsoft.EntityFrameworkCore;

namespace NotesProject.Notes.Entities;

public class NotesProjectContext : DbContext
{
  public NotesProjectContext(DbContextOptions<NotesProjectContext> options) : base(options)
  {
  }
  public DbSet<Note> Notes { get; init; }
}