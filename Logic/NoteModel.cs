using BL;
using Common.Utils;

namespace Logic;

public interface INoteModel : INote
{
    DateTime EditedAt { get; }
}

internal class BaseNoteModel : Notifiable, INote
{
    private string _header;
    private string _text;

    internal BaseNoteModel(string guid, string header, string text, DateTime editedAt)
    {
        Guid = guid;
        Header = header;
        Text = text;
        EditedAt = editedAt;

        PropertyChanged += (_, _) => EditedAt = DateTime.Now;
    }

    internal BaseNoteModel(BaseNoteModel baseNoteModel) 
        : this(baseNoteModel.Guid, baseNoteModel.Header, baseNoteModel.Text, baseNoteModel.EditedAt)
    {

    }

    public string Guid { get; }

    public string Header
    {
        get => _header;
        set => SetAndRaise(ref _header, value);
    }

    public string Text
    {
        get => _text;
        set => SetAndRaise(ref _text, value);
    }

    public DateTime EditedAt { get; private set; }
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
}