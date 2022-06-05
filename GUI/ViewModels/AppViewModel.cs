using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BL;
using Common.Utils;

namespace GUI.ViewModels;

// TODO: Do something with casts. возможно переделать некоторые интерфейсы обратно в классы.
// TODO: Например в IFolderModel было бы классно сделать IKeyedCollection<INoteModel> Notes, чтобы избавиться от ебли с кастами

// TODO: Restore from recycle bin
// TODO: Implement moving. Maybe I should inherit FolderWrapper from IFolder and implement AddNote? idk seems really complicated
// TODO: Write tests
// TODO: Move to archive by button
// TODO: Add custom folders

public interface IAppViewModel : IViewModel
{
    event Action ViewModelLoaded;
    event Action<IFolder> FolderPickRequest;

    event Action<INote> NoteAddRequest;
    event Action<INote> NoteRemoveRequest;
    event Action<INote> NoteEditRequest;

    IEnumerable<IFolderViewModel> Folders { get; set; }
    IFolderViewModel CurrentFolder { get; set; }
    INoteViewModel CurrentNote { get; set; }
}

internal partial class AppViewModel : Notifiable, IAppViewModel
{
    private static IEventManager EventManager => ((App)Application.Current).EventManager;

    private NoteViewModel _currentNote;
    private FolderViewModel _currentFolder;
    private IEnumerable<FolderViewModel> _folders;

    public AppViewModel()
    {
        if (EventManager is null)
            throw new NotSupportedException("Wrong app entry point");

        EventManager.SetViewModel(this);
        SetCommands();
        ViewModelLoaded!.Invoke();
        FolderPickRequest!.Invoke(Folders.First());
    }

    public event Action ViewModelLoaded;

    public event Action<IFolder> FolderPickRequest;

    public event Action<INote> NoteAddRequest;
    public event Action<INote> NoteRemoveRequest;
    public event Action<INote> NoteEditRequest;
    //public event Action<INoteViewModel> NoteMoveRequest;

    IEnumerable<IFolderViewModel> IAppViewModel.Folders
    {
        get => Folders;
        set => Folders = value.OfType<FolderViewModel>();
    }

    public IEnumerable<FolderViewModel> Folders
    {
        get => _folders;
        private set => SetAndRaise(ref _folders, value);
    }

    IFolderViewModel IAppViewModel.CurrentFolder
    {
        get => CurrentFolder;
        set => CurrentFolder = (FolderViewModel)value;
    }

    public FolderViewModel CurrentFolder
    {
        get => _currentFolder;
        private set => SetAndRaise(ref _currentFolder, value);
    }

    INoteViewModel IAppViewModel.CurrentNote
    {
        get => CurrentNote;
        set => CurrentNote = (NoteViewModel)value;
    }

    public NoteViewModel CurrentNote
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