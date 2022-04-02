using Logic.Dumping;
using Logic.Utils.Observable.Dict;

namespace Logic;

using BL;

public class Board : IBoard
{
    internal Board(IDumper dumper)
    {
        _dumper = dumper;

        _folders = new ObservableDict<string, Folder>();
        _folders.Changed += (_, _) => _dumper.Save(this);
        
        // TEMP
        var unsorted = new Folder(_dumper, this, "Unsorted");
        AddFolder(unsorted);
        // TEMP
    }

    internal Board(IDumper dumper, IBoard board)
    {
        _dumper = dumper;
        var rawFolders = 
            board.Folders
                .Select(f => new Folder(_dumper, this, f))
                .ToDictionary(f => f.Name, f => f);
        
        _folders = new ObservableDict<string, Folder>(rawFolders);
        _folders.Changed += (_, _) => _dumper.Save(this);
    }

    private readonly IDumper _dumper;

    public IEnumerable<IFolder> Folders => _folders.Values;
    
    private readonly ObservableDict<string, Folder> _folders;

    public bool AddFolder(IFolder folder)
    {
        if (_folders.ContainsKey(folder.Name))
            return false;

        _folders.Add(folder.Name, new Folder(_dumper, this, folder));
        return true;
    }
    
    public bool EditFolder(string oldName, string newName)
    {
        var contains = _folders.ContainsKey(oldName);
        if (!contains)
            return false;

        var folder = _folders[oldName];
        _folders.Remove(oldName);
        
        var newFolder = new Folder(_dumper, this, newName, folder.Notes);
        _folders.Add(newName, newFolder);
        return true;
    }

    public bool RemoveFolder(string name)
    {
        return _folders.Remove(name);
    }
}