using System.Windows.Input;
using GUI.Commands;

namespace GUI.ViewModels
{
    internal partial class AppViewModel
    {
        public ICommand CreateNoteCommand { get; private set; }
        public ICommand OpenNoteCommand { get; private set; }
        public ICommand CloseNoteCommand { get; private set; }
        public ICommand DeleteNoteCommand { get; private set; }
        
        public ICommand PickFolderCommand { get; private set; }

        private void SetCommands()
        {
            CreateNoteCommand = new Command(CreateNoteAction);
            OpenNoteCommand = new Command<NoteViewModel>(OpenNoteAction, OpenNoteCondition);
            CloseNoteCommand = new Command(CloseNoteAction);
            DeleteNoteCommand = new Command<NoteViewModel>(DeleteNoteAction, DeleteNoteCondition);

            PickFolderCommand = new Command<FolderViewModel>(PickFolderAction, PickFolderCondition);
        }

        private void CreateNoteAction() => NoteAddRequest!.Invoke(new NoteViewModel());

        private void OpenNoteAction(NoteViewModel note) => CurrentNote = note;

        private static bool OpenNoteCondition(NoteViewModel note) => note is not null;

        private void CloseNoteAction() => NoteEditRequest!.Invoke(CurrentNote);

        private void DeleteNoteAction(NoteViewModel note) => NoteRemoveRequest!.Invoke(note);

        private static bool DeleteNoteCondition(NoteViewModel note) => note is not null;

        private void PickFolderAction(FolderViewModel f) => FolderPickRequest!.Invoke(f);

        private static bool PickFolderCondition(FolderViewModel f) => f is not null;
    }
}
