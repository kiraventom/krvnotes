using BL;
using Common.Utils;

namespace GUI.ViewModels;

public class NoteViewModel : Notifiable, INote
{
    private string _header;
    private string _text;

    public static NoteViewModel FromINote(INote note) => new(note);

    internal NoteViewModel()
    {
    }

    private NoteViewModel(INote note)
    {
        Guid = note.Guid;
        Header = note.Header;
        Text = note.Text;
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
}