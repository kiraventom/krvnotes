using System;
using BL;

namespace GUI;

public class NoteModel : BasicNotifiable, INote
{
    public NoteModel()
    {
        Guid = System.Guid.NewGuid().ToString();
        EditedAt = DateTime.Now;
    }

    public NoteModel(INote note)
    {
        Guid = note.Guid;
        Header = note.Header;
        Text = note.Text;
        EditedAt = note.EditedAt;
    }
    
    public NoteModel(string guid) => Guid = guid;

    private string _header;
    private string _text;
    private DateTime _editedAt;

    public string Guid { get; }

    public string Header
    {
        get => _header;
        set
        {
            _header = value;
            OnPropertyChanged();
        }
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }

    public DateTime EditedAt
    {
        get => _editedAt;
        set
        {
            _editedAt = value;
            OnPropertyChanged();
        }
    }
}