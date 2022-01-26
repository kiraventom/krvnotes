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
        CreateNoteCommand = new Command(() => CurrentNote = new NoteModel());
        SaveNoteCommand = new Command(SaveNote);
        DeleteNoteCommand = new Command<NoteModel>(note => Notes.Remove(note), note => note is not null);
    }

    public ObservableCollection<NoteModel> Notes { get; set; }
    private NoteModel _currentNote;

    public NoteModel CurrentNote
    {
        get => _currentNote;
        set
        {
            _currentNote = value;
            OnPropertyChanged(nameof(CurrentNote));
            OnPropertyChanged(nameof(IsNoteEditActive));
        }
    }

    public bool IsNoteEditActive => CurrentNote is not null;

    public ICommand CreateNoteCommand { get; }
    public ICommand SaveNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }

    private void SaveNote()
    {
        if (!string.IsNullOrWhiteSpace(CurrentNote.Header) ||
            !string.IsNullOrWhiteSpace(CurrentNote.Text))
        {
            Notes.Add(CurrentNote);
        }

        CurrentNote = null;
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