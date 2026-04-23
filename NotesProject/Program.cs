using Microsoft.EntityFrameworkCore;
using NotesProject;
using NotesProject.Config;
using NotesProject.Notes.Entities;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment;

if (environment.IsDevelopment())
{
  builder.Configuration
    .AddJsonFile($"appsettings.${builder.Environment}.json",true, false);
}
else
{
  builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
}



builder.Configuration
  .GetSection(nameof(NotesProjectConfig))
  .Bind(new NotesProjectConfig());


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi()
  .AddControllers();

builder.Services.AddEndpointsApiExplorer()
  .AddSwaggerGen();


builder.Services.AddDbContext<NotesProjectContext>(options =>
{
  // get project config
  var projectConfig = builder.Configuration.GetSection(nameof(NotesProjectConfig)).Get<NotesProjectConfig>();
  if (projectConfig == null)
  {
    throw new ArgumentNullException(nameof(projectConfig));
  }
  Console.WriteLine($"Config: {projectConfig.SqlConnectionString}");
  options.UseNpgsql(projectConfig.SqlConnectionString);
});



// setup handlers
builder.Services.ConfigureServices();


var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
  });
}


app.Run();
