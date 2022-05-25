using System.Text.Json;
using BL;
using BL.Model;
using Common;

namespace Logic.Dumping;

internal class Dumper : IDumper
{
    internal Dumper()
    {
        Directory.CreateDirectory(Constants.FolderPath);
    }

    internal IBoardModel CreateBoard()
    {
        if (!File.Exists(Constants.FilePath)) 
            return new BoardModel(this);
        
        string json = File.ReadAllText(Constants.FilePath);
        var loadedBoard = JsonSerializer.Deserialize<DtoBoardWrapper>(json, Constants.SerializerOptions);
        if (loadedBoard is null)
            throw new FormatException($"Could not parse {Constants.FilePath}");

        return new BoardModel(this, loadedBoard);
    }

    void IDumper.Save(IBoardModel boardModel) => Save(boardModel);

    private static void Save(IBoardModel boardModel)
    {
        var dumpBoard = new DtoBoardWrapper(boardModel);
        string json = JsonSerializer.Serialize(dumpBoard, Constants.SerializerOptions);
        File.WriteAllText(Constants.FilePath, json);
    }
}

public interface IDumper
{
    internal void Save(IBoardModel boardModel);
}