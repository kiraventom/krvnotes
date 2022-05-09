using System.Windows.Input;
using GUI.Commands;

namespace GUI.ViewModel
{
    public partial class BoardViewModel
    {
        public ICommand CreateNoteCommand { get; private set; }
        public ICommand OpenNoteCommand { get; private set; }
        public ICommand CloseNoteCommand { get; private set; }
        public ICommand DeleteNoteCommand { get; private set; }
        
        public ICommand PickFolderCommand { get; private set; }

        private void SetCommands()
        {
            CreateNoteCommand = new Command(CreateNoteAction);
            OpenNoteCommand = new Command<NoteWrapper>(OpenNoteAction, OpenNoteCondition);
            CloseNoteCommand = new Command(CloseNoteAction);
            DeleteNoteCommand = new Command<NoteWrapper>(DeleteNoteAction, DeleteNoteCondition);

            PickFolderCommand = new Command<FolderWrapper>(PickFolderAction, PickFolderCondition);
        }

        private void CreateNoteAction()
        {
            var note = new NoteWrapper();
            CurrentFolder.Notes.Add(note);
            CurrentNote = note;
        }

        private void OpenNoteAction(NoteWrapper note) => CurrentNote = note;

        private static bool OpenNoteCondition(NoteWrapper note) => note is not null;

        private void CloseNoteAction() => CurrentNote = null;

        private void DeleteNoteAction(NoteWrapper note) => CurrentFolder.Notes.Remove(note);

        private static bool DeleteNoteCondition(NoteWrapper note) => note is not null;

        private void PickFolderAction(FolderWrapper f) => CurrentFolder = f;

        private static bool PickFolderCondition(FolderWrapper f) => f is not null;
    }
}
