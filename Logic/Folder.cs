using System.Collections;
using System.ComponentModel;
using BL;
using Common;
using Common.Utils;
using Common.Utils.Observable;
using Common.Utils.Observable.Dict;
using Logic.Dumping;

namespace Logic;

internal class Folder : IFolder
{
    private readonly IDumper _dumper;
    private readonly IBoard _board;
    private readonly ObservableDict<string, Note> _notes;
    
    public static Folder CreateCustom(IDumper dumper, IBoard board, string name)
    {
        return new Folder(dumper, board, System.Guid.NewGuid().ToString(), name, Constants.FolderType.Custom, Enumerable.Empty<DtoNoteWrapper>());
    }

    public static Folder CreateDefault(IDumper dumper, IBoard board, DefaultFolder defaultFolder)
    {
        return new Folder(dumper, board, defaultFolder.Guid, defaultFolder.Name, defaultFolder.FolderType, Enumerable.Empty<DtoNoteWrapper>());
    }

    public static Folder Load(IDumper dumper, IBoard board, DtoFolderWrapper loadedFolder)
    {
        return new Folder(dumper, board, loadedFolder.Guid, loadedFolder.Name, loadedFolder.FolderType, loadedFolder.Notes);
    }

    private Folder(IDumper dumper, IBoard board, string guid, string name, Constants.FolderType folderType, IEnumerable<DtoNoteWrapper> loadedNotes)
    {
        _dumper = dumper;
        _board = board;

        var rawNotes = loadedNotes.ToDictionary(dto => dto.Guid, Note.FromBaseNote);

        Guid = guid;
        Name = name;
        FolderType = folderType;

        _notes = new ObservableDict<string, Note>(rawNotes);
        _notes.Changed += OnNotesCollectionChanged;
        _notes.Values.ForEach(n => n.PropertyChanged += OnNoteEdited);
        
        Notes = new NotesCollection(_notes);
    }

    public string Guid { get; }

    public string Name { get; }

    public INotesCollection Notes { get; }

    public Constants.FolderType FolderType { get; }

    public event Action<INote, IFolder> NoteMoved;

    public INote AddNote(string header, string text)
    {
        var note = new Note(header, text);
        _notes.Add(note.Guid, note);
        return note;
    }

    public void MoveNote(string noteGuid, string folderGuid)
    {
        var note = _notes[noteGuid];
        _notes.Remove(noteGuid);
        var folder = _board.Folders[folderGuid] as Folder;
        folder!.AddNote(note);
        NoteMoved?.Invoke(note, folder);
    }

    public bool RemoveNote(string guid)
    {
        if (FolderType == Constants.FolderType.RecycleBin)
            return _notes.Remove(guid);

        MoveNote(guid, Constants.DefaultFolders[Constants.FolderType.RecycleBin].Guid);
        return true;
    }

    private void AddNote(Note note)
    {
        _notes.Add(note.Guid, note);
    }

    private void OnNotesCollectionChanged(object sender, DictChangedEventArgs<string, Note> e)
    {
        var note = e.Value;
        switch (e.Change)
        {
            case CollectionChangeType.Add:
                note.PropertyChanged += OnNoteEdited;
                break;
            case CollectionChangeType.Remove:
                note.PropertyChanged -= OnNoteEdited;
                break;

            default:
                throw new NotImplementedException();
        }

        _dumper.Save(_board);
    }
    
    private void OnNoteEdited(object o, PropertyChangedEventArgs propertyChangedEventArgs) => _dumper.Save(_board);
}

internal class NotesCollection : INotesCollection
{
    private readonly ObservableDict<string, Note> _notes;

    public NotesCollection(ObservableDict<string, Note> notes)
    {
        _notes = notes;
    }

    public INote this[string key] => _notes[key];

    public IEnumerator<INote> GetEnumerator() => _notes.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
