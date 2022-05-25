using System.Collections;
using BL.Model;
using Common;
using Common.Utils;
using Common.Utils.Observable.Dict;
using Logic.Dumping;

namespace Logic;

using BL;

public class BoardModel : IBoardModel
{
    private readonly IDumper _dumper;

    private readonly ObservableDict<string, FolderModel> _folders;
    
    /// <summary>
    /// Create new board.
    /// </summary>
    internal BoardModel(IDumper dumper)
    {
        _dumper = dumper;

        _folders = new ObservableDict<string, FolderModel>();
        _folders.Changed += (_, _) => _dumper.Save(this);

        Folders = new FoldersCollection(_folders);
        Constants.DefaultFolders.Values.ForEach(AddDefaultFolder);
    }

    /// <summary>
    /// Load board.
    /// </summary>
    internal BoardModel(IDumper dumper, DtoBoardWrapper loadedBoard)
    {
        _dumper = dumper;

        var folders = loadedBoard.Folders
            .ToDictionary(dto => dto.Guid, dto => FolderModel.Load(_dumper, this, dto));

        if (Constants.DefaultFolders.Keys.Any(type => folders.Values.All(f => f.FolderType != type)))
            throw new NotSupportedException("Default folders were not found");

        _folders = new ObservableDict<string, FolderModel>(folders);
        _folders.Changed += (_, _) => _dumper.Save(this);
        Folders = new FoldersCollection(_folders);
    }

    public IFoldersCollection Folders { get; }

    public bool AddFolder(string name)
    {
        var folder = FolderModel.CreateCustom(_dumper, this, name);
        _folders.Add(folder.Guid, folder);
        return true;
    }

    private void AddDefaultFolder(DefaultFolder defaultFolder)
    {
        if (_folders.ContainsKey(defaultFolder.Guid))
            throw new NotSupportedException();

        var folder = FolderModel.CreateDefault(_dumper, this, defaultFolder);
        _folders.Add(folder.Guid, folder);
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
    private readonly ObservableDict<string, FolderModel> _folders;

    public FoldersCollection(ObservableDict<string, FolderModel> folders)
    {
        _folders = folders;
    }

    public IFolderModel this[string key] => _folders[key];

    public IEnumerator<IFolderModel> GetEnumerator() => _folders.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}