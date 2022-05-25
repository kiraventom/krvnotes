using System;
using BL;
using BL.Model;
using BL.ViewModel;
using Common.Utils;

namespace GUI;

public class NoteViewModel : Notifiable, INoteViewModel
{
    private string _guid;
    private string _header;
    private string _text;

    public NoteViewModel()
    {
    }

    private NoteViewModel(INoteModel noteModel)
    {
        Guid = noteModel.Guid;
        Header = noteModel.Header;
        Text = noteModel.Text;
    }

    public static NoteViewModel FromNote(INoteModel noteModel) => new(noteModel);

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