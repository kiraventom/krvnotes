using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using GUI.Commands;

namespace GUI;

public class BoardViewModel : INotifyPropertyChanged
{
    public BoardViewModel()
    {
        Notes = new ObservableCollection<NoteModel>();
        CreateNoteCommand = new Command(CreateNewNote);
        SaveNoteCommand = new Command<NoteModel>(SaveNote, note => note is not null);
        DeleteNoteCommand = new Command<NoteModel>(DeleteNote, note => note is not null);
        
        Notes.Add(new NoteModel() {Header = "Header", Text = "Text!!!"});
        Notes.Add(new NoteModel() {Header = "Кошка любимая", Text = "Я люблю её очень сильно мяу мур мяу!!!"});
    }

    private void DeleteNote(NoteModel note) => Notes.Remove(note);

    private NoteModel _currentNote;

    public event PropertyChangedEventHandler PropertyChanged;

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
    
    public ICommand CreateNoteCommand { get; }
    public ICommand SaveNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }

    private void CreateNewNote()
    {
        CurrentNote = new NoteModel() {Header = "HeaderTest", Text = "TextTest"};
    }

    private void SaveNote(NoteModel note)
    {
        Notes.Add(note);
        CurrentNote = null;
    }
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null) 
            return;
        
        var e = new PropertyChangedEventArgs(propertyName);
        PropertyChanged(this, e);
    }
}