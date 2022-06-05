using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BL;
using Common.Utils;

namespace GUI.ViewModels;

// TODO: Пофиксить невозможность нажать кнопку удаления из-за "неоткрываемости" заметок в корзине
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

    IKeyedCollection<FolderViewModel> Folders { get; set; }
    string CurrentFolderGuid { get; set; }
    string CurrentNoteGuid { get; set; }
}

internal partial class AppViewModel : Notifiable, IAppViewModel
{
    private static IEventManager EventManager => ((App)Application.Current).EventManager;

    private NoteViewModel _currentNote;
    private FolderViewModel _currentFolder;
    private IKeyedCollection<FolderViewModel> _folders;
    private string _currentFolderGuid;
    private string _currentNoteGuid;

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

    public IKeyedCollection<FolderViewModel> Folders
    {
        get => _folders;
        set
        {
            SetAndRaise(ref _folders, value);
            OnPropertyChanged(nameof(CurrentFolder));
        }
    }

    public string CurrentFolderGuid
    {
        get => _currentFolderGuid;
        set
        {
            _currentFolderGuid = value;
            OnPropertyChanged(nameof(CurrentFolder));
        }
    }

    public string CurrentNoteGuid
    {
        get => _currentNoteGuid;
        set
        {
            _currentNoteGuid = value;
            OnPropertyChanged(nameof(CurrentNote));
            OnPropertyChanged(nameof(IsEditorOpen));
        }
    }

    public FolderViewModel CurrentFolder => Folders[CurrentFolderGuid];
    public NoteViewModel CurrentNote => CurrentNoteGuid is not null ? CurrentFolder.Notes[CurrentNoteGuid] : null;

    public bool IsEditorOpen => CurrentNote is not null;
}