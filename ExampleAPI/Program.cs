using ExampleAPI.Service;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton<IFileService, FileService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

// http://localhost:5239/swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.UseStaticFiles();
app.MapGet("/", async context =>
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Description.html");
    context.Response.ContentType = "text/html; charset=utf-8";

    await context.Response.SendFileAsync(filePath);
});

app.Run();