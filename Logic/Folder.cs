using System.Collections;
using System.ComponentModel;
using BL;
using Common.Utils;
using Common.Utils.Observable;
using Common.Utils.Observable.Dict;
using Logic.Dumping;

namespace Logic;

internal class Folder : IFolder
{
    private readonly ObservableDict<string, Note> _notes;
    
    internal Folder(IDumper dumper, IBoard board, string name)
    {
        Name = name;
        _notes = new ObservableDict<string, Note>();
        _notes.Changed += (_, _) => dumper.Save(board);
        Notes = new NotesWrapper(_notes);
    }

    internal Folder(IDumper dumper, IBoard board, string name, DtoFolderWrapper loadedFolder)
    {
        var rawNotes = loadedFolder.Notes
            .ToDictionary(n => n.Guid, n => new Note(n));

        Name = name;
        _notes = new ObservableDict<string, Note>(rawNotes);

        void OnNoteEdited(object o, PropertyChangedEventArgs propertyChangedEventArgs) => dumper.Save(board);
        void OnNotesCollectionChanged(object _, DictChangedEventArgs<string, Note> e)
        {
            var note = e.Value;
            if (e.Change == CollectionChangeType.Add) 
                note.PropertyChanged += OnNoteEdited;
            
            if (e.Change == CollectionChangeType.Remove) 
                note.PropertyChanged -= OnNoteEdited;

            dumper.Save(board);
        }

        _notes.Changed += OnNotesCollectionChanged;
        _notes.Values.ForEach(n => n.PropertyChanged += OnNoteEdited);
        
        Notes = new NotesWrapper(_notes);
    }

    public string Name { get; }

    public INotesCollection Notes { get; }

    public INote AddNote(string header, string text)
    {
        var note = new Note(header, text);
        _notes.Add(note.Guid, note);
        return note;
    }

    public bool RemoveNote(string guid)
    {
        return _notes.Remove(guid);
    }
}

internal class NotesWrapper : INotesCollection
{
    private readonly ObservableDict<string, Note> _notes;

    public NotesWrapper(ObservableDict<string, Note> notes)
    {
        _notes = notes;
    }

    public INote this[string key] => _notes[key];

    public IEnumerator<INote> GetEnumerator() => _notes.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
