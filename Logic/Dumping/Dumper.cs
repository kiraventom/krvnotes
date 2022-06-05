using System.Text.Json;
using BL;
using Common;

namespace Logic.Dumping;

internal class Dumper : IDumper
{
    internal Dumper()
    {
        Directory.CreateDirectory(Constants.FolderPath);
    }

    internal BoardModel CreateBoard()
    {
        if (!File.Exists(Constants.FilePath)) 
            return new BoardModel(this);
        
        string json = File.ReadAllText(Constants.FilePath);
        var loadedBoard = JsonSerializer.Deserialize<DtoBoardWrapper>(json, Constants.SerializerOptions);
        if (loadedBoard is null)
            throw new FormatException($"Could not parse {Constants.FilePath}");

        return new BoardModel(this, loadedBoard);
    }

    void IDumper.Save(BoardModel boardModel) => Save(boardModel);

    private static void Save(BoardModel boardModel)
    {
        var dumpBoard = new DtoBoardWrapper(boardModel);
        string json = JsonSerializer.Serialize(dumpBoard, Constants.SerializerOptions);
        File.WriteAllText(Constants.FilePath, json);
    }
}

public interface IDumper
{
    internal void Save(BoardModel boardModel);
}