using System.ComponentModel;
using BL;
using Common;
using Common.Utils;
using Common.Utils.Observable;
using Common.Utils.Observable.Dict;
using Logic.Dumping;

namespace Logic;

public interface IFolderModel : IFolder
{
    INoteModel AddNote(string header, string text);
    void MoveNote(string noteGuid, string folderGuid);
    bool RemoveNote(string guid);
}

internal class FolderModel : IFolderModel
{
    private readonly IDumper _dumper;
    private readonly IBoardModel _boardModel;
    private readonly ObservableDict<string, NoteModel> _notes;
    
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

        var rawNotes = loadedNotes.ToDictionary(dto => dto.Guid, NoteModel.FromBaseNote);

        Guid = guid;
        Name = name;
        FolderType = folderType;

        _notes = new ObservableDict<string, NoteModel>(rawNotes);
        _notes.Changed += OnNotesCollectionChanged;
        _notes.Values.ForEach(n => n.PropertyChanged += OnNoteEdited);
        
        Notes = new KeyedCollection<NoteModel, INoteModel>(_notes);
    }

    public string Guid { get; }

    public string Name { get; }

    IEnumerable<INote> IFolder.Notes => Notes;

    public IKeyedCollection<INoteModel> Notes { get; }

    public Constants.FolderType FolderType { get; }

    public INoteModel AddNote(string header, string text)
    {
        var note = new NoteModel(header, text);
        _notes.Add(note.Guid, note);
        return note;
    }

    public void MoveNote(string noteGuid, string folderGuid)
    {
        var note = _notes[noteGuid];
        _notes.Remove(noteGuid);
        var folder = _boardModel.Folders[folderGuid] as FolderModel;
        folder!.AddNote(note);
    }

    public bool RemoveNote(string guid)
    {
        if (FolderType == Constants.FolderType.RecycleBin)
            return _notes.Remove(guid);

        MoveNote(guid, Constants.DefaultFolders[Constants.FolderType.RecycleBin].Guid);
        return true;
    }

    private void AddNote(NoteModel noteModel)
    {
        _notes.Add(noteModel.Guid, noteModel);
    }

    private void OnNotesCollectionChanged(object sender, DictChangedEventArgs<string, NoteModel> e)
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