using BL;
using Logic.Dumping;
using Logic.Utils.Observable.Dict;

namespace Logic;

internal class Folder : IFolder
{
    internal Folder(IDumper dumper, IBoard board, IFolder folder)
        : this(dumper, board, folder.Name, folder.Notes)
    {
    }

    internal Folder(IDumper dumper, IBoard board, string name)
        : this(dumper, board, name, Enumerable.Empty<INote>())
    {
    }

    internal Folder(IDumper dumper, IBoard board, string name, IEnumerable<INote> notes)
    {
        Name = name;
        var rawNotes = notes
            .Select(n => new Note(n))
            .ToDictionary(n => n.Guid, n => n);

        _rawNotes = new ObservableDict<string, Note>(rawNotes);
        _rawNotes.Changed += (_, _) => dumper.Save(board);
    }

    public string Name { get; }

    public IEnumerable<INote> Notes => _rawNotes.Values;
    private readonly ObservableDict<string, Note> _rawNotes;

    public bool AddNote(INote note)
    {
        var rawNote = new Note(note);

        if (_rawNotes.ContainsKey(rawNote.Guid))
            return false;

        _rawNotes.Add(rawNote.Guid, rawNote);
        return true;
    }

    public bool EditNote(INote note)
    {
        var rawNote = new Note(note);

        var contains = _rawNotes.ContainsKey(rawNote.Guid);
        if (!contains)
            return false;

        _rawNotes[rawNote.Guid] = rawNote;
        return true;
    }

    public bool RemoveNote(string guid)
    {
        return _rawNotes.Remove(guid);
    }
}