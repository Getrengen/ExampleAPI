namespace ExampleAPI.Service.FileS;

public class FileService : IFileService
{
    private readonly string _currentDirectory;

    public FileService()
    {
        _currentDirectory = Path.Combine(Environment.CurrentDirectory, "Files");

        if (!Directory.Exists(_currentDirectory))
            Directory.CreateDirectory(_currentDirectory);
    }
    
    public Task<Stream?> GetFile(string name)
    {
        string fileName = Path.GetFileName(name);
        string filePath = Path.Combine(_currentDirectory, fileName);

        if (!File.Exists(filePath)|| "TestAPIBD_test_data.db" == fileName )
            return Task.FromResult<Stream?>(null);

        Stream stream = File.OpenRead(filePath);
        return Task.FromResult<Stream?>(stream);
    }

    public async Task<bool> SaveFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        string fileName = Path.GetFileName(file.FileName);
        string filePath = Path.Combine(_currentDirectory, fileName);
        if (File.Exists(filePath))
            return false;
        using FileStream stream = new FileStream(filePath, FileMode.CreateNew);
        await file.CopyToAsync(stream);
        
        return File.Exists(filePath);
    }
}