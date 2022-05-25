using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BL.Model;
using BL.ViewModel;
using Common.Utils.Exceptions;
using GUI.ViewModel;

namespace Starter
{
    public class EventManager : IEventManager
    {
        private IViewModel _viewModel;
        private readonly IModel _model;

        public EventManager(IModel model)
        {
            _model = model;
        }

        public void SetViewModel(IViewModel viewModel)
        {
            if (_viewModel is not null)
                throw new NotSupportedException("ViewModel is already set");

            _viewModel = viewModel;
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
