using System.Text.Json;

namespace Logic;

internal static class Dumper
{
    static Dumper()
    {
        Directory.CreateDirectory(Constants.FolderPath);
    }
    
    public static void Save(Board board)
    {
        string json = JsonSerializer.Serialize(board.Folders, Constants.SerializerOptions);
        File.WriteAllText(Constants.FilePath, json);    
    }

    internal static bool TryLoad(out Board board)
    {
        if (!File.Exists(Constants.FilePath))
        {
            board = null;
            return false;
        }
        
        string json = File.ReadAllText(Constants.FilePath);
        
        var loadedFolders = JsonSerializer.Deserialize<IEnumerable<Folder>>(json, Constants.SerializerOptions);
        ArgumentNullException.ThrowIfNull(loadedFolders); // I know it's not argument, idc
        board = new Board(loadedFolders);
        return true;
    }
}