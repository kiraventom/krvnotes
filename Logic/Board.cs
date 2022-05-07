using Common.Utils.Observable.Dict;
using Logic.Dumping;

namespace Logic;

using BL;

public class Board : IBoard
{
    private readonly IDumper _dumper;

    private readonly ObservableDict<string, Folder> _folders;

    internal Board(IDumper dumper)
    {
        _dumper = dumper;

        _folders = new ObservableDict<string, Folder>();
        _folders.Changed += (_, _) => _dumper.Save(this);
        
        // TEMP
        AddFolder("Unsorted");
        // TEMP
    }

    internal Board(IDumper dumper, DtoBoardWrapper loadedBoard)
    {
        _dumper = dumper;

        var folders = 
            loadedBoard.Folders
                .ToDictionary(f => f.Name, f => new Folder(_dumper, this, f.Name, f));
        
        _folders = new ObservableDict<string, Folder>(folders);
        _folders.Changed += (_, _) => _dumper.Save(this);
    }

    public IEnumerable<IFolder> Folders => _folders.Values;

    public bool AddFolder(string name)
    {
        if (_folders.ContainsKey(name))
            return false;

        _folders.Add(name, new Folder(_dumper, this, name));
        return true;
    }

    public bool RemoveFolder(string name)
    {
        return _folders.Remove(name);
    }
}