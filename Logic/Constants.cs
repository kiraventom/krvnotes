using System.Text.Json;

namespace Logic;

internal static class Constants
{
    public const string AppName = "krvnotes";
    
    public static readonly string FolderPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            AppName);

    public const string Filename = "board.json";    
        
    public static readonly string FilePath =
        Path.Combine(FolderPath, Filename);

    public static readonly JsonSerializerOptions SerializerOptions = new (){WriteIndented = true};

    public static readonly Random Random = new();
}