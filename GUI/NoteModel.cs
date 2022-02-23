namespace GUI;

public class NoteModel : BasicNotifier
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
}