using System.Text.Json;
using BL;

namespace Logic.Dumping;

public class Dumper : IDumper
{
    public Dumper()
    {
        Directory.CreateDirectory(Constants.FolderPath);
    }
    
    public IBoard CreateBoard()
    {
        if (!File.Exists(Constants.FilePath)) 
            return new Board(this);
        
        string json = File.ReadAllText(Constants.FilePath);
        var loadedBoard = JsonSerializer.Deserialize<DtoBoardWrapper>(json, Constants.SerializerOptions);
        if (loadedBoard is null)
            throw new FormatException($"Could not parse {Constants.FilePath}");

        return new Board(this, loadedBoard);
    }

    void IDumper.Save(IBoard board) => Save(board);

    private static void Save(IBoard board)
    {
        var dumpBoard = new DtoBoardWrapper(board);
        string json = JsonSerializer.Serialize(dumpBoard, Constants.SerializerOptions);
        File.WriteAllText(Constants.FilePath, json);
    }
}

public interface IDumper
{
    internal void Save(IBoard board);
}