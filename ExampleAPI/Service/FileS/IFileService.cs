namespace ExampleAPI.Service.FileS;

public interface IFileService
{
    Task<Stream?> GetFile(string name);
    Task<bool> SaveFile(IFormFile file);
    
}