using BL;
using Common.Utils.Exceptions;
using GUI.ViewModels;
using Logic;

namespace Starter
{
    internal class EventManager : IEventManager
    {
        private IAppViewModel _viewModel;
        private readonly Model _model;

        private FolderModel CurrentFolderModel => _model.BoardModel.Folders[_viewModel.CurrentFolderGuid];

        public EventManager(Model model)
        {
            _model = model;
        }
        
        public void SetViewModel(IViewModel viewModel)
        {
            ArgumentTypeException.ThrowIfNotTypeOf<IAppViewModel>(viewModel, out var boardViewModel);

            if (_viewModel is not null)
                throw new NotSupportedException("ViewModel is already set");

            _viewModel = boardViewModel;

            _viewModel.ViewModelLoaded += OnModelLoaded;

            _viewModel.NoteCreateRequest += OnNoteCreateRequest;
            _viewModel.NoteRemoveRequest += OnNoteRemoveRequest;
        }

        private void OnModelLoaded()
        {
            _viewModel.FoldersGetter = () => _model.BoardModel.Folders;
        }

        private void OnNoteCreateRequest()
        {
            var noteModel = CurrentFolderModel.CreateNote();
            _viewModel.CurrentNoteGuid = noteModel.Guid;
        }

        private void OnNoteRemoveRequest(INote noteToDelete)
        {
            var wasDeleted = CurrentFolderModel.RemoveNote(noteToDelete.Guid);
        }
    }
}
