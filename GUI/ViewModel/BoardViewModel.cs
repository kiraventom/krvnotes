using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Common.Utils;

namespace GUI.ViewModel;

// TODO: Disable adding in archive and recycle bin
// TODO: Move to recycle bin on delete
// TODO: Move to archive by button
// TODO: Add custom folders

public partial class BoardViewModel : Notifiable
{
    private static App App => Application.Current as App;

    private NoteWrapper _currentNote;
    private FolderWrapper _currentFolder;

    public BoardViewModel()
    {
        if (App!.Controller is null)
            throw new NotSupportedException("Wrong app entry point");

        SetCommands();
        var board = App.Controller.Board;

        var folders = board.Folders.Select(FolderWrapper.FromFolder);
        Folders = new ObservableCollection<FolderWrapper>(folders);
        
        App.Controller.SetViewModel(this);
        AfterLoad();
    }

    private void AfterLoad()
    {
        CurrentFolder = Folders.First();
    }

    public event Action<FolderWrapper, FolderWrapper> ActiveFolderChanged;

    public ObservableCollection<FolderWrapper> Folders { get; }

    public FolderWrapper CurrentFolder
    {
        get => _currentFolder;
        internal set
        {
            var oldFolder = _currentFolder;
            SetAndRaise(ref _currentFolder, value);
            ActiveFolderChanged?.Invoke(oldFolder, value);
        }
    }

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
}