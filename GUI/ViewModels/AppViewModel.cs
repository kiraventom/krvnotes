using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BL;
using Common.Utils;

namespace GUI.ViewModels;

// TODO: Restore from recycle bin
// TODO: Implement moving. Maybe I should inherit FolderWrapper from IFolder and implement AddNote? idk seems really complicated
// TODO: Write tests
// TODO: Move to archive by button
// TODO: Add custom folders

// Этот интерфейс нужен для сокрытия от EventManager публичного ctor вьюмодели, который нельзя сделать internal из-за специфики WPF
public interface IAppViewModel : IViewModel
{
    event Action ViewModelLoaded;
    event Action<IFolder> FolderPickRequest;

    event Action<INote> NoteAddRequest;
    event Action<INote> NoteRemoveRequest;
    event Action<INote> NoteEditRequest;

    IEnumerable<FolderViewModel> Folders { get; set; }
    FolderViewModel CurrentFolder { get; set; }
    NoteViewModel CurrentNote { get; set; }
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

        PickFolderCommand.Execute(Folders.First());
    }

    public event Action ViewModelLoaded;

    public event Action<IFolder> FolderPickRequest;

    public event Action<INote> NoteAddRequest;
    public event Action<INote> NoteRemoveRequest;
    public event Action<INote> NoteEditRequest;
    //public event Action<NoteViewModel> NoteMoveRequest;

    public IEnumerable<FolderViewModel> Folders
    {
        get => _folders;
        set => SetAndRaise(ref _folders, value);
    }

    public FolderViewModel CurrentFolder
    {
        get => _currentFolder;
        set => SetAndRaise(ref _currentFolder, value);
    }

    public NoteViewModel CurrentNote
    {
        get => _currentNote;
        set
        {
            SetAndRaise(ref _currentNote, value);
            OnPropertyChanged(nameof(IsEditorOpen));
        }
    }

    public bool IsEditorOpen => CurrentNote is not null;
}