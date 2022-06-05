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
            _viewModel.FolderPickRequest += OnFolderPickRequest;

            _viewModel.NoteAddRequest += OnNoteAddRequest;
            _viewModel.NoteRemoveRequest += OnNoteRemoveRequest;
            _viewModel.NoteEditRequest += OnNoteEditRequest;
        }

        private void OnModelLoaded()
        {
            var modelFolders = _model.BoardModel.Folders;
            var viewModelFolders = modelFolders.Cast(FolderViewModel.FromIFolder);
            _viewModel.Folders = viewModelFolders;
        }

        private void OnFolderPickRequest(IFolder pickedFolder)
        {
            // maybe obsolete
            var folderModel = _model.BoardModel.Folders[pickedFolder.Guid];
            _viewModel.CurrentFolderGuid = folderModel.Guid;
        }

        private void OnNoteAddRequest(INote noteToAdd)
        {
            var noteModel = CurrentFolderModel.AddNote(noteToAdd.Header, noteToAdd.Text);
            UpdateFolders();
            _viewModel.CurrentNoteGuid = noteModel.Guid;
        }

        private void OnNoteRemoveRequest(INote noteToDelete)
        {
            var wasDeleted = CurrentFolderModel.RemoveNote(noteToDelete.Guid);
            UpdateFolders();
        }

        private void OnNoteEditRequest(INote noteToEdit)
        {
            var noteModel = CurrentFolderModel.Notes[noteToEdit.Guid];
            noteModel.Edit(noteToEdit.Header, noteToEdit.Text);
            UpdateFolders();
            _viewModel.CurrentNoteGuid = null;
        }

        private void UpdateFolders()
        {
            _viewModel.Folders = _model.BoardModel.Folders.Cast(FolderViewModel.FromIFolder);
        }
    }
}
