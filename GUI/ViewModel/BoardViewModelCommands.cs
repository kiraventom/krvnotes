using System.Windows.Input;
using GUI.Templates;

namespace GUI.ViewModel
{
    public partial class BoardViewModel
    {
        // Commands
        public ICommand CreateNoteCommand { get; }
        public ICommand OpenNoteCommand { get; }
        public ICommand CloseNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }

        private void CreateAction()
        {
            var note = new NoteWrapper();
            Notes.Add(note);
            CurrentNote = note;
        }

        private void OpenAction(NoteWrapper note) => CurrentNote = note;

        private static bool OpenCondition(NoteWrapper note) => note is not null;

        private void CloseAction() => CurrentNote = null;

        private void DeleteAction(NoteWrapper note) => Notes.Remove(note);

        private static bool DeleteCondition(NoteWrapper note) => note is not null;
    }
}
