using NotesProject.Notes;

namespace NotesProject;

public static class ServiceConfiguration
{
  public static IServiceCollection ConfigureServices(this IServiceCollection services)
  {
    // setup handlers
    services.AddSingleton<INotesHandler, NotesHandler>();
    return services;
  }
}