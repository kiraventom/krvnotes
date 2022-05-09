using BL;
using Common.Utils;

namespace Logic;

public class BaseNote : Notifiable
{
    private string _header;
    private string _text;
    private DateTime _editedAt;
    
    internal BaseNote(string guid, string header, string text, DateTime editedAt)
    {
        Guid = guid;
        Header = header;
        Text = text;
        EditedAt = editedAt;
    }

    internal BaseNote(BaseNote baseNote) : this(baseNote.Guid, baseNote.Header, baseNote.Text, baseNote.EditedAt)
    {

    }

    public string Guid { get; }

    public string Header
    {
        get => _header;
        protected set => SetAndRaise(ref _header, value);
    }

    public string Text
    {
        get => _text;
        protected set => SetAndRaise(ref _text, value);
    }

    public DateTime EditedAt
    {
        get => _editedAt;
        protected set => SetAndRaise(ref _editedAt, value);
    }
}

public class Note : BaseNote, INote
{
    public Note(string header, string text)
        : base(System.Guid.NewGuid().ToString(), header, text, DateTime.Now)
    {
    }

    private Note(BaseNote baseNote) : base(baseNote)
    {
    }

    public static Note FromBaseNote(BaseNote baseNote) => new Note(baseNote);

    public void Edit(string header, string text)
    {
        Header = header;
        Text = text;
        EditedAt = DateTime.Now;
    }
}