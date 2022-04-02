using BL;

namespace Logic;

public class Note : INote
{
    public Note(INote note)
    {
        Guid = note.Guid;
        Header = note.Header;
        Text = note.Text;
        EditedAt = note.EditedAt;
    }

    public string Guid { get; }
    public string Header { get; }
    public string Text { get; }
    public DateTime EditedAt { get; }
}