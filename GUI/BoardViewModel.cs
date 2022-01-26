using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GUI.Commands;
using Logic;

namespace GUI;

// TODO: Add notes editing
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
        Notes.CollectionChanged += NotesOnCollectionChanged;
        
        CreateNoteCommand = new Command(() => CurrentNote = new NoteModel());
        SaveNoteCommand = new Command(SaveNote);
        DeleteNoteCommand = new Command<NoteModel>(note => Notes.Remove(note), note => note is not null);
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
    public ICommand SaveNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }

    
    // Commands methods
    private void SaveNote()
    {
        if (!string.IsNullOrWhiteSpace(CurrentNote.Header) ||
            !string.IsNullOrWhiteSpace(CurrentNote.Text))
        {
            Notes.Add(CurrentNote);
        }

        CurrentNote = null;
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
    
    private void NotesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
            {
                var note = (NoteModel)e.NewItems![0];
                Controller.Board.Add(note!.Guid, note.Header, note.Text);
                break;
            }

            case NotifyCollectionChangedAction.Remove:
            {
                var note = (NoteModel)e.OldItems![0];
                Controller.Board.Remove(note!.Guid);
                break;
            }
        }

        Dumper.Save(Controller.Board);
    }
}