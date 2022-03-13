using System.Text.Json.Serialization;
using BL;

namespace Logic;

public class Note : INote
{
    public Note(INote note) : this(note.Guid, note.Header, note.Text, note.EditedAt)
    {
        
    }
    
    [JsonConstructor]
    public Note(string guid, string header, string text, DateTime editedAt)
    {
        Guid = guid;
        Header = header;
        Text = text;
        EditedAt = editedAt;
    }

    public string Guid { get; }
    public string Header { get; }
    public string Text { get; }
    public DateTime EditedAt { get; }
}