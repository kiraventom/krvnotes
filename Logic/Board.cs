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
    
    /// <summary>
    /// Create new board.
    /// </summary>
    internal Board(IDumper dumper)
    {
        _dumper = dumper;

        _folders = new ObservableDict<string, Folder>();
        _folders.Changed += (_, _) => _dumper.Save(this);

        Folders = new FoldersCollection(_folders);
        Constants.DefaultFolders.ForEach(df => AddDefaultFolder(df));
    }

    /// <summary>
    /// Load board.
    /// </summary>
    internal Board(IDumper dumper, DtoBoardWrapper loadedBoard)
    {
        _dumper = dumper;

        var folders = loadedBoard.Folders
            .ToDictionary(dto => dto.Guid, dto => Folder.Load(_dumper, this, dto));

        if (Constants.DefaultFolders.Any(df => folders.Values.All(f => f.FolderType != df.FolderType)))
            throw new NotSupportedException("Default folders were not found");

        _folders = new ObservableDict<string, Folder>(folders);
        _folders.Changed += (_, _) => _dumper.Save(this);
        Folders = new FoldersCollection(_folders);
    }

    public IFoldersCollection Folders { get; }

    public bool AddFolder(string name)
    {
        var folder = Folder.CreateCustom(_dumper, this, name);
        _folders.Add(folder.Guid, folder);
        return true;
    }

    private bool AddDefaultFolder(DefaultFolder defaultFolder)
    {
        if (_folders.ContainsKey(defaultFolder.Guid))
            throw new NotSupportedException();

        var folder = Folder.CreateDefault(_dumper, this, defaultFolder);
        _folders.Add(folder.Guid, folder);
        return true;
    }

    public bool RemoveFolder(string guid)
    {
        if (_folders[guid].FolderType != Constants.FolderType.Custom)
            throw new NotSupportedException();

        return _folders.Remove(guid);
    }
}

internal class FoldersCollection : IFoldersCollection
{
    private readonly ObservableDict<string, Folder> _folders;

    public FoldersCollection(ObservableDict<string, Folder> folders)
    {
        _folders = folders;
    }

    public IFolder this[string key] => _folders[key];

    public IEnumerator<IFolder> GetEnumerator() => _folders.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}