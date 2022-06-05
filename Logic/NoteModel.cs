using BL;
using Common.Utils;

namespace Logic;

public interface INoteModel : INote
{
    void Edit(string header, string text);
    
    DateTime EditedAt { get; }
}

internal class BaseNoteModel : Notifiable
{
    private string _header;
    private string _text;
    private DateTime _editedAt;
    
    internal BaseNoteModel(string guid, string header, string text, DateTime editedAt)
    {
        Guid = guid;
        Header = header;
        Text = text;
        EditedAt = editedAt;
    }

    internal BaseNoteModel(BaseNoteModel baseNoteModel) : this(baseNoteModel.Guid, baseNoteModel.Header, baseNoteModel.Text, baseNoteModel.EditedAt)
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

internal class NoteModel : BaseNoteModel, INoteModel
{
    public NoteModel(string header, string text)
        : base(System.Guid.NewGuid().ToString(), header, text, DateTime.Now)
    {
    }

    private NoteModel(BaseNoteModel baseNoteModel) : base(baseNoteModel)
    {
    }

    public static NoteModel FromBaseNote(BaseNoteModel baseNoteModel) => new(baseNoteModel);

    public void Edit(string header, string text)
    {
        Header = header;
        Text = text;
        EditedAt = DateTime.Now;
    }
}