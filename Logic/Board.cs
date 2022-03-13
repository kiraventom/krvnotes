using System.Linq;

namespace Logic;

using BL;

public class Board : IBoard
{
    // TODO: move these methods to IFolder, implement here folder list editing
    // TODO: implement ReadOnlyDictionaryWrapper to support upcasting https://stackoverflow.com/a/13602918/10466326
    public static void OnStartup()
    {
        Controller.BoardRequested += (_, _) =>
        {
            var didLoad = Dumper.TryLoad(out var board);
            Controller.SetBoard(didLoad ? board : new Board());
        };
    }

    private Board()
    {
        _folders = new List<Folder>();
        Folder unsorted;
        unsorted = new Folder(nameof(unsorted));
        _folders.Add(unsorted);
    }

    internal Board(IEnumerable<Folder> loadedFolders)
    {
        _folders = loadedFolders.ToList();
    }

    public IEnumerable<IFolder> Folders => _folders.AsReadOnly();
    private readonly List<Folder> _folders;

    bool IBoard.AddNote(IFolder folder, INote note)
    {
        var rawFolder = GetRawFolder(folder);
        return rawFolder is not null && AddNote(rawFolder, new Note(note));
    }

    private bool AddNote(Folder folder, Note note)
    {
        if (folder.Notes.Any(n => n.Guid == note.Guid))
            return false;
        
        folder.Notes.Add(note);

        Dumper.Save(this);
        return true;
    }

    bool IBoard.EditNote(IFolder folder, INote note)
    {
        var rawFolder = GetRawFolder(folder);
        return rawFolder is not null && EditNote(rawFolder, new Note(note));
    }
    
    private bool EditNote(Folder folder, Note note)
    {
        var index = folder.Notes.FindIndex(n => n.Guid == note.Guid);
        if (index == -1)
            return false;
        
        folder.Notes[index] = note;

        Dumper.Save(this);
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
        Dumper.Save(this);
        
        return true;
    }

    private Folder GetRawFolder(IFolder folder)
    {
        return _folders.FirstOrDefault(f => f.Name == folder.Name);
    }
}