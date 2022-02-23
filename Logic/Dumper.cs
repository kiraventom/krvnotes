using System.Text.Json;
using BL;

namespace Logic;

internal static class Dumper
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
        ArgumentNullException.ThrowIfNull(loadedNotes); // I know it's not argument, idc
        board = new Board(loadedNotes.ToDictionary(p => p.Key, p => p.Value as INote));
        return true;
    }
}