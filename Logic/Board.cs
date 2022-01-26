namespace Logic;

// TODO: Remove 'Note' class completely and replace it with NoteModel
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

    public void AddOrReplace(string guid, string header, string text)
    {
        var note = new Note(header, text, DateTime.Now);
        _notes[guid] = note;

        Dumper.Save(this);
    }

    public bool Remove(string guid)
    {
        var didDelete = _notes.Remove(guid);
        
        Dumper.Save(this);
        return didDelete;
    }
}

public record Note(string Header, string Text, DateTime CreatedAt);