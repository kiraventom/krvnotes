using System.Collections;
using Common;
using Common.Utils;
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

        Folders = new FoldersWrapper(_folders);
        Constants.DefaultFolders.ForEach(name => AddFolder(name));
    }

    internal Board(IDumper dumper, DtoBoardWrapper loadedBoard)
    {
        _dumper = dumper;

        var folders = loadedBoard.Folders
            .ToDictionary(dto => dto.Name, dto => new Folder(_dumper, this, dto.Name, dto));

        if (Constants.DefaultFolders.Any(name => !folders.ContainsKey(name)))
            throw new NotSupportedException("Default folders were not found");

        _folders = new ObservableDict<string, Folder>(folders);
        _folders.Changed += (_, _) => _dumper.Save(this);
        Folders = new FoldersWrapper(_folders);
    }

    public IFoldersCollection Folders { get; }

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

internal class FoldersWrapper : IFoldersCollection
{
    private readonly ObservableDict<string, Folder> _folders;

    public FoldersWrapper(ObservableDict<string, Folder> folders)
    {
        _folders = folders;
    }

    public IFolder this[string key] => _folders[key];

    public IEnumerator<IFolder> GetEnumerator() => _folders.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}