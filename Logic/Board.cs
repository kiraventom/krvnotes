namespace Logic;

public class Board
{
    public Board()
    {
        _notes = new Dictionary<string, Note>();
    }
    
    public Board(IDictionary<string, Note> loadedNotes)
    {
        _notes = new Dictionary<string, Note>(loadedNotes);
    }

    public IReadOnlyDictionary<string, Note> Notes => _notes;
    private readonly Dictionary<string, Note> _notes;

    public void Add(string guid, string header, string text)
    {
        var note = new Note(header, text, DateTime.Now);
        _notes.Add(guid, note);
    }

    public bool Remove(string guid) => _notes.Remove(guid);
}

public record Note(string Header, string Text, DateTime CreatedAt);