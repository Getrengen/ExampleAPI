namespace ExampleAPI.Infrastructure;

public class SQLiteDBManager
{
    public static string ConnectionString { get; private set; } = string.Empty;

    private static readonly string FileName = "TestAPIBD_test_data.db";
    private static readonly string AppFolder = "TestAPI";

    public static void Initialize()
    {
        string dbPath = GetTargetPath();

        if (!File.Exists(dbPath))
        {
            string sourcePath = GetSourcePath();

            if (!File.Exists(sourcePath))
                throw new FileNotFoundException($"Не найдена исходная БД: {sourcePath}");
            
            File.Copy(sourcePath, dbPath);
        }

        ConnectionString = $"Data Source={dbPath}";
        //Console.WriteLine(ConnectionString);
    }

    private static string GetTargetPath()
    {
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            AppFolder);

        Directory.CreateDirectory(folder);

        return Path.Combine(folder, FileName);
    }

    private static string GetSourcePath()
    {
        return $"{AppContext.BaseDirectory}\\Infrastructure\\{FileName}";
    }

}