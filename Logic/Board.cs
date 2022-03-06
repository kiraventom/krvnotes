namespace Logic;

using BL;

public class Board : IBoard
{
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
        _notes = new Dictionary<string, INote>();
    }

    internal Board(IDictionary<string, INote> loadedNotes)
    {
        _notes = new Dictionary<string, INote>(loadedNotes);
    }

    public IReadOnlyDictionary<string, INote> Notes => _notes;
    private readonly Dictionary<string, INote> _notes;

    public bool Add(string guid, string header, string text)
    {
        if (_notes.ContainsKey(guid))
            return false;
        
        _notes[guid] = new Note(header, text, DateTime.Now);

        Dumper.Save(this);
        return true;
    }
    
    public bool Edit(string guid, string header, string text)
    {
        if (!_notes.ContainsKey(guid))
            return false;
        
        _notes[guid] = new Note(header, text, DateTime.Now);

        Dumper.Save(this);
        return true;
    }

    public bool Remove(string guid)
    {
        var didDelete = _notes.Remove(guid);
        
        if (didDelete)
            Dumper.Save(this);
        return didDelete;
    }
}

public record Note(string Header, string Text, DateTime CreatedAt) : INote;