using BL;
using BL.Model;
using Common.Utils;

namespace Logic;

public class BaseNoteModel : Notifiable
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

public class NoteModelModel : BaseNoteModel, INoteModel
{
    public NoteModelModel(string header, string text)
        : base(System.Guid.NewGuid().ToString(), header, text, DateTime.Now)
    {
    }

    private NoteModelModel(BaseNoteModel baseNoteModel) : base(baseNoteModel)
    {
    }

    public static NoteModelModel FromBaseNote(BaseNoteModel baseNoteModel) => new(baseNoteModel);

    public void Edit(string header, string text)
    {
        Header = header;
        Text = text;
        EditedAt = DateTime.Now;
    }
}