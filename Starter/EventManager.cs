﻿using BL;
using Common.Utils.Exceptions;
using GUI.ViewModels;
using Logic;

namespace Starter
{
    internal class EventManager : IEventManager
    {
        private IAppViewModel _viewModel;
        private readonly IModel _model;

        private IFolderModel CurrentFolderModel => (IFolderModel)_model.BoardModel.Folders[_viewModel.CurrentFolder.Guid];

        public EventManager(IModel model)
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
            var viewModelFolders = modelFolders.Select(IFolderViewModel.FromIFolder);
            _viewModel.Folders = viewModelFolders;
        }

        private void OnFolderPickRequest(IFolder pickedFolder)
        {
            var modelFolder = _model.BoardModel.Folders[pickedFolder.Guid];
            _viewModel.CurrentFolder = IFolderViewModel.FromIFolder(modelFolder);
        }

        private void OnNoteAddRequest(INote noteToAdd)
        {
            var modelNote = CurrentFolderModel.AddNote(noteToAdd.Header, noteToAdd.Text);
            _viewModel.CurrentFolder = IFolderViewModel.FromIFolder(CurrentFolderModel);
            _viewModel.CurrentNote = INoteViewModel.FromINote(modelNote);
        }

        private void OnNoteRemoveRequest(INote noteToDelete)
        {
            var wasDeleted = CurrentFolderModel.RemoveNote(noteToDelete.Guid);
            _viewModel.CurrentFolder = IFolderViewModel.FromIFolder(CurrentFolderModel);
        }

        private void OnNoteEditRequest(INote noteToEdit)
        {
            var noteModel = ((IKeyedCollection<INoteModel>)CurrentFolderModel.Notes)[noteToEdit.Guid];
            noteModel.Edit(noteToEdit.Header, noteToEdit.Text);
            _viewModel.CurrentFolder = IFolderViewModel.FromIFolder(CurrentFolderModel);
            _viewModel.CurrentNote = null;
        }
    }

    //internal class EventManager : IEventManager
    //{
    //    private BoardViewModel _viewModel;

    //    public EventManager(IBoard board)
    //    {
    //        Board = board;

    //        Board.Folders.ForEach(f => f.NoteMoved += OnModelNoteMoved);
    //    }

    //    public IBoard Board { get; }

    //    private IFolder ModelCurrentFolder => Board.Folders[_viewModel.CurrentFolder.Guid];

    //    public void SetViewModel(object viewModel)
    //    {
    //        ArgumentTypeException.ThrowIfNotTypeOf<BoardViewModel>(viewModel, out var boardViewModel);

    //        if (_viewModel is not null)
    //            throw new NotSupportedException("ViewModel is already set");

    //        _viewModel = boardViewModel;
    //        _viewModel.Folders.CollectionChanged += OnViewModelFoldersCollectionChanged;
    //        _viewModel.CurrentFolderChanged += OnViewModelCurrentFolderChanged;
    //    }

    //    private void OnViewModelFoldersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        // TODO: On custom folders 
    //        // TODO: Do not forget to subscribe to OnModelNoteMoved
    //    }

    //    private void OnViewModelCurrentFolderChanged(FolderWrapper oldFolder, FolderWrapper newFolder)
    //    {
    //        if (oldFolder is not null) // first run
    //        {
    //            oldFolder.Notes.CollectionChanged -= OnViewModelNotesCollectionChanged;
    //            oldFolder.Notes.ForEach(n => n.PropertyChanged -= OnViewModelNoteEdited);
    //        }

    //        newFolder.Notes.CollectionChanged += OnViewModelNotesCollectionChanged;
    //        newFolder.Notes.ForEach(n => n.PropertyChanged += OnViewModelNoteEdited);
    //    }

    //    private void OnViewModelNotesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        switch (e.Action)
    //        {
    //            case NotifyCollectionChangedAction.Add:
    //                foreach (NoteWrapper newNote in e.NewItems!)
    //                {
    //                    var addedNote = ModelCurrentFolder.AddNote(newNote.Header, newNote.Text);
    //                    newNote.Guid = addedNote.Guid; // TODO:  dis shit is ugly
    //                    newNote.PropertyChanged += OnViewModelNoteEdited;
    //                }
    //                break;

    //            case NotifyCollectionChangedAction.Remove:
    //                foreach (NoteWrapper oldNote in e.OldItems!)
    //                {
    //                    ModelCurrentFolder.RemoveNote(oldNote.Guid);
    //                    oldNote.PropertyChanged -= OnViewModelNoteEdited;
    //                }
    //                break;

    //            case NotifyCollectionChangedAction.Replace:
    //            case NotifyCollectionChangedAction.Move:
    //            case NotifyCollectionChangedAction.Reset:
    //                throw new NotSupportedException();
    //        }
    //    }

    //    private void OnViewModelNoteEdited(object sender, PropertyChangedEventArgs e)
    //    {
    //        ArgumentTypeException.ThrowIfNotTypeOf<NoteWrapper>(sender, out var noteWrapper);
    //        ModelCurrentFolder.Notes[noteWrapper.Guid].Edit(noteWrapper.Header, noteWrapper.Text);
    //    }

    //    // TODO: Выглядит уродливо
    //    private void OnModelNoteMoved(INote movedNote, IFolder folderMovedTo)
    //    {
    //        var viewModelFolder = _viewModel.Folders.First(f => f.Guid == folderMovedTo.Guid);
    //        viewModelFolder.Notes.CollectionChanged -= OnViewModelNotesCollectionChanged;

    //        viewModelFolder.Notes.Add(NoteWrapper.FromNote(movedNote));

    //        viewModelFolder.Notes.CollectionChanged += OnViewModelNotesCollectionChanged;
    //    }
    //}
}
