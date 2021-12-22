using System.Collections.Immutable;

namespace Logic;

public class Board
{
    internal Board()
    {
        _notes = new Dictionary<long, Note>();
    }

    public IReadOnlyDictionary<long, Note> Notes => _notes;
    private readonly Dictionary<long, Note> _notes;

    public void Add(string header, string text)
    {
        long id;
        do
        {
            id = Constants.Random.Next();
        } 
        while (_notes.ContainsKey(id));
        
        _notes.Add(id, new Note(id, header, text, DateTime.Now));
    }

    public bool Remove(long id) => _notes.Remove(id);
}

public record Note(long Id, string Header, string Text, DateTime CreatedAt);