using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BL;
using Common.Utils;
using GUI.Commands;

namespace GUI.ViewModel;

// TODO: Implement folders properly

public partial class BoardViewModel : Notifiable
{
    private static App App => Application.Current as App;

    private readonly IBoard _board;
    private NoteWrapper _currentNote;
    private IFolder _currentFolder;

    public BoardViewModel()
    {
        if (App!.Controller is null)
            throw new NotSupportedException("Wrong app entry point");

        App.Controller.SetViewModel(this);
        _board = App.Controller.Board;

        // TEMP
        var folders = _board.Folders;
        var unsorted = folders.First(f => f.Name == "Unsorted");
        CurrentFolder = unsorted;
        var notes = unsorted.Notes.Select(note => new NoteWrapper(note));
        // TEMP
        
        Notes = new ObservableCollection<NoteWrapper>(notes);

        CreateNoteCommand = new Command(CreateAction);
        OpenNoteCommand = new Command<NoteWrapper>(OpenAction, OpenCondition);
        CloseNoteCommand = new Command(CloseAction);
        DeleteNoteCommand = new Command<NoteWrapper>(DeleteAction, DeleteCondition);

        ViewModelBuilt?.Invoke();
    }

    public event Action ViewModelBuilt;
    
    public ObservableCollection<NoteWrapper> Notes { get; }

    public NoteWrapper CurrentNote
    {
        get => _currentNote;
        internal set
        {
            SetAndRaise(ref _currentNote, value);
            OnPropertyChanged(nameof(IsNoteEditActive));
        }
    }

    public bool IsNoteEditActive => CurrentNote is not null;

    public IFolder CurrentFolder
    {
        get => _currentFolder;
        internal set => _currentFolder = value;
    }
}