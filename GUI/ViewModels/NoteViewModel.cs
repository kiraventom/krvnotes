using System;
using System.ComponentModel;
using BL;
using Common.Utils;

namespace GUI.ViewModels;

public interface INoteViewModel : INote
{
    static INoteViewModel FromINote(INote note) => new NoteViewModel(note);

    event PropertyChangedEventHandler PropertyChanged;
}

internal class NoteViewModel : Notifiable, INoteViewModel
{
    private string _header;
    private string _text;

    public NoteViewModel()
    {
    }

    internal NoteViewModel(INote note)
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