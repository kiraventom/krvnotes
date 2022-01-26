using System;
using System.ComponentModel;

namespace GUI;

public class NoteModel : INotifyPropertyChanged
{
    public NoteModel()
    {
        Guid = System.Guid.NewGuid().ToString();
    }
    
    public NoteModel(string guid) => Guid = guid;

    private string _header;
    private string _text;
    
    public string Guid { get; }

    public string Header
    {
        get => _header;
        set
        {
            _header = value;
            OnPropertyChanged(nameof(Header));
        }
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged(nameof(Text));
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null) 
            return;
        
        var e = new PropertyChangedEventArgs(propertyName);
        PropertyChanged(this, e);
    }
}