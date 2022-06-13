using System;
using System.Linq;
using System.Windows;
using BL;
using Common;
using Common.Utils;

namespace GUI.ViewModels;

// TODO: Пофиксить невозможность нажать кнопку удаления из-за "неоткрываемости" заметок в корзине
// TODO: Restore from recycle bin
// TODO: Implement moving
// TODO: Write tests
// TODO: Move to archive by button
// TODO: Add custom folders

// Этот интерфейс нужен для сокрытия от EventManager публичного ctor вьюмодели, который нельзя сделать internal из-за специфики WPF
public interface IAppViewModel : IViewModel
{
    event Action ViewModelLoaded;

    event Action NoteCreateRequest;
    event Action<INote> NoteRemoveRequest;
    
    Func<IKeyedCollection<IFolder>> FoldersGetter { get; set; }
    string CurrentFolderGuid { get; set; }
    string CurrentNoteGuid { get; set; }
}

internal partial class AppViewModel : Notifiable, IAppViewModel
{
    private static IEventManager EventManager => ((App)Application.Current).EventManager;
    
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

    public event Action NoteCreateRequest;
    public event Action<INote> NoteRemoveRequest;

    public Func<IKeyedCollection<IFolder>> FoldersGetter { get; set; }

    //public event Action<NoteViewModel> NoteMoveRequest;

    public IKeyedCollection<IFolder> Folders => FoldersGetter.Invoke();

    public string CurrentFolderGuid
    {
        get => _currentFolderGuid;
        set
        {
            _currentFolderGuid = value;
            OnPropertyChanged(nameof(CurrentFolder));
            OnPropertyChanged(nameof(CanUserCreate));
            OnPropertyChanged(nameof(CanUserEdit));
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

    public IFolder CurrentFolder => Folders[CurrentFolderGuid];
    public INote CurrentNote => CurrentNoteGuid is not null ? CurrentFolder.Notes[CurrentNoteGuid] : null;

    public bool IsEditorOpen => CurrentNote is not null;

    public bool CanUserCreate => CurrentFolder.FolderType is not Constants.FolderType.Archive and not Constants.FolderType.RecycleBin;
    private bool CanUserEdit => CurrentFolder.FolderType is not Constants.FolderType.RecycleBin;
}