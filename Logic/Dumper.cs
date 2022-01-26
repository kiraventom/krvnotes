using System.Text.Json;

namespace Logic;

public static class Dumper
{
    static Dumper()
    {
        Directory.CreateDirectory(Constants.FolderPath);
    }
    
    public static void Save(Board board)
    {
        string json = JsonSerializer.Serialize(board.Notes, Constants.SerializerOptions);
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
        var loadedNotes = JsonSerializer.Deserialize<IDictionary<string, Note>>(json, Constants.SerializerOptions);
        board = new Board(loadedNotes);
        return true;
    }
}