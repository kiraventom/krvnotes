using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

    private IFolder CurrentFolder => Board.Folders[_viewModel.CurrentFolder.Name];

    public void SetViewModel(object viewModel)
    { 
        ArgumentTypeException.ThrowIfNotTypeOf<BoardViewModel>(viewModel, out var boardViewModel);
        
        if (_viewModel is not null)
            throw new NotSupportedException("ViewModel is already set");
        
        _viewModel = boardViewModel;
        _viewModel.Folders.CollectionChanged += OnFoldersCollectionChanged;
        _viewModel.ActiveFolderChanged += OnActiveFolderChanged;
    }

    private void OnFoldersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        // TODO
    }

    private void OnActiveFolderChanged(FolderWrapper oldFolder, FolderWrapper newFolder)
    {
        if (oldFolder is not null) // first run
        {
            oldFolder.Notes.CollectionChanged -= OnNotesCollectionChanged;
            oldFolder.Notes.ForEach(n => n.PropertyChanged -= OnNoteEdited);
        }

        newFolder.Notes.CollectionChanged += OnNotesCollectionChanged;
        newFolder.Notes.ForEach(n => n.PropertyChanged += OnNoteEdited);
    }

    private void OnNotesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (NoteWrapper newNote in e.NewItems!)
                {
                    var addedNote = CurrentFolder.AddNote(newNote.Header, newNote.Text);
                    newNote.Guid = addedNote.Guid; // TODO:  dis shit is ugly
                    newNote.PropertyChanged += OnNoteEdited;
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                foreach (NoteWrapper oldNote in e.OldItems!)
                {
                    CurrentFolder.RemoveNote(oldNote.Guid);
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
        CurrentFolder.Notes[noteWrapper.Guid].Edit(noteWrapper.Header, noteWrapper.Text);
    }
}