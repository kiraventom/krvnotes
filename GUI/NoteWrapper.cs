using System;
using BL;
using Common.Utils;

namespace GUI;

public class NoteWrapper : Notifiable
{
    private string _guid;
    private string _header;
    private string _text;

    public NoteWrapper()
    {
    }

    public NoteWrapper(INote note)
    {
        Guid = note.Guid;
        Header = note.Header;
        Text = note.Text;
    }

    public string Guid
    {
        get => _guid;
        set
        {
            if (!string.IsNullOrWhiteSpace(_guid))
                throw new NotSupportedException("Guid is already set");

            _guid = value;
        }
    }

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