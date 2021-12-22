using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GUI;

public class BoardViewModel : INotifyPropertyChanged
{
    public BoardViewModel()
    {
        Notes = new ObservableCollection<NoteModel>();
        Notes.Add(new NoteModel() {Header = "Header", Text = "Text!!!"});
        Notes.Add(new NoteModel() {Header = "Кошка любимая", Text = "Я люблю её очень сильно мяу мур мяу!!!"});
    }
    
    private NoteModel _currentNote;

    public ObservableCollection<NoteModel> Notes { get; set; }
    public NoteModel CurrentNote
    {
        get => _currentNote;
        set
        {
            _currentNote = value;
            OnPropertyChanged(nameof(CurrentNote));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null) 
            return;
        
        var e = new PropertyChangedEventArgs(propertyName);
        PropertyChanged(this, e);
    }
}