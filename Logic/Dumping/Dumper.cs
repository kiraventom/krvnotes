using System.Text.Json;
using BL;

namespace Logic.Dumping;

public partial class Dumper : IDumper
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
        var dumpBoard = JsonSerializer.Deserialize<DumpBoard>(json, Constants.SerializerOptions);
        if (dumpBoard is null)
            throw new FormatException($"Could not parse {Constants.FilePath}");

        return new Board(this, dumpBoard);
    }

    void IDumper.Save(IBoard board) => Save(board);

    private static void Save(IBoard board)
    {
        var dumpBoard = new DumpBoard(board.Folders);
        string json = JsonSerializer.Serialize(dumpBoard, Constants.SerializerOptions);
        File.WriteAllText(Constants.FilePath, json);
    }
}

public interface IDumper
{
    internal void Save(IBoard board);
}