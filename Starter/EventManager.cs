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

        private FolderModel CurrentFolderModel => _model.BoardModel.Folders[_viewModel.CurrentFolder.Guid];

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
            _viewModel.FolderPickRequest += OnFolderPickRequest;

            _viewModel.NoteAddRequest += OnNoteAddRequest;
            _viewModel.NoteRemoveRequest += OnNoteRemoveRequest;
            _viewModel.NoteEditRequest += OnNoteEditRequest;
        }

        private void OnModelLoaded()
        {
            var modelFolders = _model.BoardModel.Folders;
            var viewModelFolders = modelFolders.Select(FolderViewModel.FromIFolder);
            _viewModel.Folders = viewModelFolders;
        }

        private void OnFolderPickRequest(IFolder pickedFolder)
        {
            var folderModel = _model.BoardModel.Folders[pickedFolder.Guid];
            _viewModel.CurrentFolder = FolderViewModel.FromIFolder(folderModel);
        }

        private void OnNoteAddRequest(INote noteToAdd)
        {
            var noteModel = CurrentFolderModel.AddNote(noteToAdd.Header, noteToAdd.Text);
            _viewModel.CurrentFolder = FolderViewModel.FromIFolder(CurrentFolderModel);
            _viewModel.CurrentNote = NoteViewModel.FromINote(noteModel);
        }

        private void OnNoteRemoveRequest(INote noteToDelete)
        {
            var wasDeleted = CurrentFolderModel.RemoveNote(noteToDelete.Guid);
            _viewModel.CurrentFolder = FolderViewModel.FromIFolder(CurrentFolderModel);
        }

        private void OnNoteEditRequest(INote noteToEdit)
        {
            var noteModel = CurrentFolderModel.Notes[noteToEdit.Guid];
            noteModel.Edit(noteToEdit.Header, noteToEdit.Text);
            _viewModel.CurrentFolder = FolderViewModel.FromIFolder(CurrentFolderModel);
            _viewModel.CurrentNote = null;
        }
    }
}
