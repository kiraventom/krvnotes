using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GUI.Commands;
using Logic;

namespace GUI;

public class BoardViewModel : INotifyPropertyChanged
{
    public BoardViewModel()
    {
        Controller = new Controller();
        var loadedNotes =
            Controller.Board.Notes
                .Select(p => new NoteModel(p.Key)
                {
                    Header = p.Value.Header,
                    Text = p.Value.Text
                });

        Notes = new ObservableCollection<NoteModel>(loadedNotes);

        CreateNoteCommand = new Command(() => CurrentNote = new NoteModel());
        OpenNoteCommand = new Command<NoteModel>(note => CurrentNote = note, note => note is not null);
        SaveNoteCommand = new Command(SaveAction);
        DeleteNoteCommand = new Command<NoteModel>(DeleteAction,  note => note is not null);
    }


    // Exposed properties
    public ObservableCollection<NoteModel> Notes { get; set; }

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

    private NoteModel _currentNote;

    public bool IsNoteEditActive => CurrentNote is not null;


    // Commands
    public ICommand CreateNoteCommand { get; }
    public ICommand OpenNoteCommand { get; }
    public ICommand SaveNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }


    // Commands actions
    private void SaveAction()
    {
        if (!string.IsNullOrWhiteSpace(CurrentNote.Header) ||
            !string.IsNullOrWhiteSpace(CurrentNote.Text))
        {
            if (!Notes.Contains(CurrentNote))
                Notes.Add(CurrentNote);
            
            Controller.Board.AddOrReplace(CurrentNote.Guid, CurrentNote.Header, CurrentNote.Text);
        }

        CurrentNote = null;
    }
    
    private void DeleteAction(NoteModel note)
    {
        Notes.Remove(note);
        Controller.Board.Remove(note!.Guid);
    }


    // Events
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        var e = new PropertyChangedEventArgs(propertyName);
        PropertyChanged(this, e);
    }


    // Logic
    private Controller Controller { get; }
}