namespace Logic;

using BL;

public class Board : IBoard
{
    // TODO: move these methods to IFolder, implement here folder list editing. UPD: not possible, cuz there's nothing to pass into Dumper. Mayble later

    // TODO: implement ReadOnlyDictionaryWrapper to support upcasting https://stackoverflow.com/a/13602918/10466326
    
    public Board()
    {
        var didLoad = Dumper.TryLoad(out var folders);
        if (didLoad)
        {
            _folders = folders.ToList();
            return;
        }
        
        _folders = new List<Folder>();
        Folder unsorted;
        unsorted = new Folder(nameof(unsorted));
        AddFolder(unsorted);
    }

    public IEnumerable<IFolder> Folders => _folders.AsReadOnly();
    private readonly List<Folder> _folders;

    public bool AddFolder(IFolder folder)
    {
        if (_folders.Any(f => f.Name == folder.Name))
            return false;
        
        _folders.Add(new Folder(folder));
        Dumper.Save(Folders);
        return true;
    }

    bool IBoard.AddNote(IFolder folder, INote note)
    {
        var rawFolder = GetRawFolder(folder);
        return rawFolder is not null && AddNote(rawFolder, new Note(note));
    }

    // TODO: this shit is strange, we can "edit" all of the notes in the folder. Think abt it
    public bool EditFolder(IFolder folder)
    {
        var index = _folders.FindIndex(f => f.Name == folder.Name);
        if (index == -1)
            return false;

        _folders[index] = new Folder(folder);
        Dumper.Save(Folders);
        return true;
    }

    private bool AddNote(Folder folder, Note note)
    {
        if (folder.Notes.Any(n => n.Guid == note.Guid))
            return false;
        
        folder.Notes.Add(note);

        Dumper.Save(Folders);
        return true;
    }

    bool IBoard.EditNote(IFolder folder, INote note)
    {
        var rawFolder = GetRawFolder(folder);
        return rawFolder is not null && EditNote(rawFolder, new Note(note));
    }

    public bool RemoveFolder(string name)
    {
        var index = _folders.FindIndex(f => f.Name == name);
        if (index == -1)
            return false;
        
        _folders.RemoveAt(index);
        Dumper.Save(Folders);
        return true;
    }

    private bool EditNote(Folder folder, Note note)
    {
        var index = folder.Notes.FindIndex(n => n.Guid == note.Guid);
        if (index == -1)
            return false;
        
        folder.Notes[index] = note;

        Dumper.Save(Folders);
        return true;
    }

    bool IBoard.RemoveNote(IFolder folder, string guid)
    {
        var rawFolder = GetRawFolder(folder);
        return rawFolder is not null && RemoveNote(rawFolder, guid);
    }

    private bool RemoveNote(Folder folder, string guid)
    {
        var index = folder.Notes.FindIndex(n => n.Guid == guid);
        if (index == -1)
            return false;
        
        folder.Notes.RemoveAt(index);
        Dumper.Save(Folders);
        
        return true;
    }

    private Folder GetRawFolder(IFolder folder)
    {
        return _folders.FirstOrDefault(f => f.Name == folder.Name);
    }
}