using ExampleAPI.Service.FileS;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;
    
    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не передан");
        
        if (!await _fileService.SaveFile(file))
            return BadRequest("Не удалось сохранить файл");

        return Ok($"Файл {file.FileName} был успешно загружен");
    }

    [HttpGet("see")]
    public async Task<IActionResult> GetSee([FromQuery] string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return BadRequest("Введите имя файла");

        Stream? stream = await _fileService.GetFile(fileName);

        if (stream == null)
            return NotFound("Файл не найден");
        string extension = Path.GetExtension(fileName);

        string contentType = extension switch
        {
            ".txt" => "text/plain",
            ".html" => "text/html",
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };

        return File(stream, contentType);
    }
    
    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile([FromQuery] string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return BadRequest("Введите имя файла");

        Stream? stream = await _fileService.GetFile(fileName);

        if (stream == null)
            return NotFound("Файл не найден");

        return File(
            stream,
            "application/octet-stream",
            fileName
        );
    }
}