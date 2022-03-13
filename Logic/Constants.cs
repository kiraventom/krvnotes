using System.Text.Json;

namespace Logic;

internal static class Constants
{
    private const string AppName = "krvnotes";
    private const string Filename = "board.json";  
    
    public static readonly string FolderPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            AppName);
  
    public static readonly string FilePath = Path.Combine(FolderPath, Filename);

    public static readonly JsonSerializerOptions SerializerOptions = new ()
    {
        WriteIndented = true, 
        PropertyNameCaseInsensitive = true
    };

    public static readonly Random Random = new();
}