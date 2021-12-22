using System.Text.Json;

namespace Logic;

public class Dumper
{
    internal Dumper()
    {
        Directory.CreateDirectory(Constants.FolderPath);
    }
    
    public void Save(Board board)
    {
        string json = JsonSerializer.Serialize(board, Constants.SerializerOptions);
        File.WriteAllText(Constants.FilePath, json);    
    }

    internal bool TryLoad(out Board board)
    {
        if (!File.Exists(Constants.FilePath))
        {
            board = null;
            return false;
        }
        
        string json = File.ReadAllText(Constants.FilePath);
        board = JsonSerializer.Deserialize<Board>(json, Constants.SerializerOptions);
        return true;
    }
}