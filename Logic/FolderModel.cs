using System.Collections;
using System.ComponentModel;
using BL;
using BL.Model;
using Common;
using Common.Utils;
using Common.Utils.Observable;
using Common.Utils.Observable.Dict;
using Logic.Dumping;

namespace Logic;

internal class FolderModel : IFolderModel
{
    private readonly IDumper _dumper;
    private readonly IBoardModel _boardModel;
    private readonly ObservableDict<string, NoteModelModel> _notes;
    
    public static FolderModel CreateCustom(IDumper dumper, IBoardModel boardModel, string name)
    {
        return new FolderModel(dumper, boardModel, System.Guid.NewGuid().ToString(), name, Constants.FolderType.Custom, Enumerable.Empty<DtoNoteModelWrapper>());
    }

    public static FolderModel CreateDefault(IDumper dumper, IBoardModel boardModel, DefaultFolder defaultFolder)
    {
        return new FolderModel(dumper, boardModel, defaultFolder.Guid, defaultFolder.Name, defaultFolder.FolderType, Enumerable.Empty<DtoNoteModelWrapper>());
    }

    public static FolderModel Load(IDumper dumper, IBoardModel boardModel, DtoFolderWrapper loadedFolder)
    {
        return new FolderModel(dumper, boardModel, loadedFolder.Guid, loadedFolder.Name, loadedFolder.FolderType, loadedFolder.Notes);
    }

    private FolderModel(IDumper dumper, IBoardModel boardModel, string guid, string name, Constants.FolderType folderType, IEnumerable<DtoNoteModelWrapper> loadedNotes)
    {
        _dumper = dumper;
        _boardModel = boardModel;

        var rawNotes = loadedNotes.ToDictionary(dto => dto.Guid, NoteModelModel.FromBaseNote);

        Guid = guid;
        Name = name;
        FolderType = folderType;

        _notes = new ObservableDict<string, NoteModelModel>(rawNotes);
        _notes.Changed += OnNotesCollectionChanged;
        _notes.Values.ForEach(n => n.PropertyChanged += OnNoteEdited);
        
        Notes = new NotesCollection(_notes);
    }

    public string Guid { get; }

    public string Name { get; }

    public INotesCollection Notes { get; }

    public Constants.FolderType FolderType { get; }

    public event Action<INoteModel, IFolderModel> NoteMoved;

    public INoteModel AddNote(string header, string text)
    {
        var note = new NoteModelModel(header, text);
        _notes.Add(note.Guid, note);
        return note;
    }

    public void MoveNote(string noteGuid, string folderGuid)
    {
        var note = _notes[noteGuid];
        _notes.Remove(noteGuid);
        var folder = _boardModel.Folders[folderGuid] as FolderModel;
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

    private void AddNote(NoteModelModel noteModelModel)
    {
        _notes.Add(noteModelModel.Guid, noteModelModel);
    }

    private void OnNotesCollectionChanged(object sender, DictChangedEventArgs<string, NoteModelModel> e)
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

        _dumper.Save(_boardModel);
    }
    
    private void OnNoteEdited(object o, PropertyChangedEventArgs propertyChangedEventArgs) => _dumper.Save(_boardModel);
}

internal class NotesCollection : INotesCollection
{
    private readonly ObservableDict<string, NoteModelModel> _notes;

    public NotesCollection(ObservableDict<string, NoteModelModel> notes)
    {
        _notes = notes;
    }

    public INoteModel this[string key] => _notes[key];

    public IEnumerator<INoteModel> GetEnumerator() => _notes.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
