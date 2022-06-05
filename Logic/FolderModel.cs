using System.ComponentModel;
using BL;
using Common;
using Common.Utils;
using Common.Utils.Observable;
using Common.Utils.Observable.Dict;
using Logic.Dumping;

namespace Logic;

public class FolderModel : IFolder
{
    private readonly IDumper _dumper;
    private readonly BoardModel _boardModel;
    private readonly ObservableDict<string, NoteModel> _notes;
    
    internal static FolderModel CreateCustom(IDumper dumper, BoardModel boardModel, string name)
    {
        return new FolderModel(dumper, boardModel, System.Guid.NewGuid().ToString(), name, Constants.FolderType.Custom, Enumerable.Empty<DtoNoteModelWrapper>());
    }

    internal static FolderModel CreateDefault(IDumper dumper, BoardModel boardModel, DefaultFolder defaultFolder)
    {
        return new FolderModel(dumper, boardModel, defaultFolder.Guid, defaultFolder.Name, defaultFolder.FolderType, Enumerable.Empty<DtoNoteModelWrapper>());
    }

    internal static FolderModel Load(IDumper dumper, BoardModel boardModel, DtoFolderWrapper loadedFolder)
    {
        return new FolderModel(dumper, boardModel, loadedFolder.Guid, loadedFolder.Name, loadedFolder.FolderType, loadedFolder.Notes);
    }

    private FolderModel(IDumper dumper, BoardModel boardModel, string guid, string name, Constants.FolderType folderType, IEnumerable<DtoNoteModelWrapper> loadedNotes)
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
        
        Notes = new KeyedCollection<NoteModel>(_notes);
    }

    public string Guid { get; }
    
    public string Name { get; }

    public Constants.FolderType FolderType { get; }

    IKeyedCollection<INote> IFolder.Notes => Notes;

    public IKeyedCollection<INoteModel> Notes { get; }

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
        var folder = (FolderModel)_boardModel.Folders[folderGuid];
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