using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BL;
using BL.ViewModel;
using Common.Utils;

namespace GUI.ViewModel;

// TODO: Restore from recycle bin
// TODO: Implement moving. Maybe I should inherit FolderWrapper from IFolder and implement AddNote? idk seems really complicated
// TODO: Write tests
// TODO: Move to archive by button
// TODO: Add custom folders

public partial class ViewModel : Notifiable, IViewModel
{
    private static IEventManager EventManager => ((App)Application.Current).EventManager;

    private INoteViewModel _currentNote;
    private IFolderViewModel _currentFolder;

    public ViewModel()
    {
        if (EventManager is null)
            throw new NotSupportedException("Wrong app entry point");

        EventManager.SetViewModel(this);

        SetCommands();
        //var board = EventManager.Board;

        //var folders = board.Folders.Select(FolderViewModel.FromFolder);
        //Folders = new ObservableCollection<FolderViewModel>(folders);

        //AfterLoad();
    }

    private void AfterLoad()
    {
        CurrentFolder = Folders.First();
    }

    //public event Action<FolderViewModel, FolderViewModel> CurrentFolderChanged;

    public ObservableCollection<IFolderViewModel> Folders { get; }

    public IFolderViewModel CurrentFolder
    {
        get => _currentFolder;
        private set
        {
            var oldFolder = _currentFolder;
            SetAndRaise(ref _currentFolder, value);
            //CurrentFolderChanged?.Invoke(oldFolder, value);
        }
    }

    public INoteViewModel CurrentNote
    {
        get => _currentNote;
        private set
        {
            SetAndRaise(ref _currentNote, value);
            OnPropertyChanged(nameof(IsNoteEditActive));
        }
    }

    public bool IsNoteEditActive => CurrentNote is not null;
}