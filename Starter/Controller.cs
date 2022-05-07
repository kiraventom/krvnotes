using System.Collections.Specialized;
using System.ComponentModel;
using BL;
using Common.Utils;
using Common.Utils.Exceptions;
using GUI;
using GUI.ViewModel;

namespace Starter;

internal class Controller : IController
{
    private BoardViewModel _viewModel;

    public Controller(IBoard board)
    {
        Board = board;
    }
    
    public IBoard Board { get; }

    public void SetViewModel(object viewModel)
    { 
        ArgumentTypeException.ThrowIfNotTypeOf<BoardViewModel>(viewModel, out var boardViewModel);
        
        if (_viewModel is not null)
            throw new NotSupportedException("ViewModel is already set");
        
        _viewModel = boardViewModel;
        _viewModel.ViewModelBuilt += Subscribe;
    }

    private void Subscribe()
    {
        _viewModel.Notes.CollectionChanged += OnNotesCollectionChanged;
        _viewModel.Notes.ForEach(n => n.PropertyChanged += OnNoteEdited);
    }

    private void OnNotesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (NoteWrapper newNote in e.NewItems!)
                {
                    var addedNote = _viewModel.CurrentFolder.AddNote(newNote.Header, newNote.Text);
                    newNote.Guid = addedNote.Guid; // TODO:  dis shit is ugly
                    newNote.PropertyChanged += OnNoteEdited;
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                foreach (NoteWrapper oldNote in e.OldItems!)
                {
                    _viewModel.CurrentFolder.RemoveNote(oldNote.Guid);
                    oldNote.PropertyChanged -= OnNoteEdited;
                }
                break;

            case NotifyCollectionChangedAction.Replace:
            case NotifyCollectionChangedAction.Move:
            case NotifyCollectionChangedAction.Reset:
                throw new NotSupportedException();
        }
    }

    private void OnNoteEdited(object sender, PropertyChangedEventArgs e)
    {
        ArgumentTypeException.ThrowIfNotTypeOf<NoteWrapper>(sender, out var noteWrapper);
        _viewModel.CurrentFolder.Notes[noteWrapper.Guid].Edit(noteWrapper.Header, noteWrapper.Text);
    }
}