using System.Windows.Input;
using BL;
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
            CreateNoteCommand = new Command(CreateNoteAction, CreateNoteCondition);
            OpenNoteCommand = new Command<INote>(OpenNoteAction, OpenNoteCondition);
            CloseNoteCommand = new Command(CloseNoteAction);
            DeleteNoteCommand = new Command<INote>(DeleteNoteAction, DeleteNoteCondition);

            PickFolderCommand = new Command<IFolder>(PickFolderAction, PickFolderCondition);
        }

        private void CreateNoteAction() => NoteCreateRequest!.Invoke();

        private bool CreateNoteCondition() => CanUserCreate;

        private void OpenNoteAction(INote note) => CurrentNoteGuid = note.Guid;

        private bool OpenNoteCondition(INote note) => note is not null && CanUserEdit;

        private void CloseNoteAction() => CurrentNoteGuid = null;

        private void DeleteNoteAction(INote note) => NoteRemoveRequest!.Invoke(note);

        private static bool DeleteNoteCondition(INote note) => note is not null;

        private void PickFolderAction(IFolder f) => CurrentFolderGuid = f.Guid; 

        private static bool PickFolderCondition(IFolder f) => f is not null;
    }
}
