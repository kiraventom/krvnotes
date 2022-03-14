using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GUI.Commands;
using BL;

namespace GUI;

// TODO: Implement folders properly

public class BoardViewModel : BasicNotifiable
{
    public BoardViewModel()
    {
        var app = Application.Current as App;
        if (app!.Controller is null)
            throw new NotSupportedException("Wrong app entry point");
        
        _board = app.Controller.Board;

        var folders = _board.Folders;
        var unsorted = folders.First(f => f.Name == "unsorted");
        _currentFolder = unsorted;
        var notes = unsorted.Notes.Select(note => new NoteModel(note));

        Notes = new ObservableCollection<NoteModel>(notes);
        foreach (var note in Notes)
            note.PropertyChanged += NotesOnContentEdited;
        
        Notes.CollectionChanged += NotesOnCollectionChanged;

        CreateNoteCommand = new Command(() => CurrentNote = new NoteModel());
        OpenNoteCommand = new Command<NoteModel>(
            note => CurrentNote = note,
            note => note is not null);
        SaveNoteCommand = new Command(SaveAction);
        CancelEditingCommand = new Command(() => CurrentNote = null);
        DeleteNoteCommand = new Command<NoteModel>(
            note => Notes.Remove(note),  
            note => note is not null);
    }
    
    private readonly IBoard _board;
    private NoteModel _currentNote;
    private IFolder _currentFolder;

    // Exposed properties
    public ObservableCollection<NoteModel> Notes { get; }

    public NoteModel CurrentNote
    {
        get => _currentNote;
        set
        {
            _currentNote = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsNoteEditActive));
        }
    }

    public bool IsNoteEditActive => CurrentNote is not null;

    // Commands
    public ICommand CreateNoteCommand { get; }
    public ICommand OpenNoteCommand { get; }
    public ICommand SaveNoteCommand { get; }
    public ICommand CancelEditingCommand { get; }
    public ICommand DeleteNoteCommand { get; }

    // Commands
    private void SaveAction()
    {
        if (!Notes.Contains(CurrentNote))
            Notes.Add(CurrentNote);
        
        CurrentNote = null;
    }

    // Event handlers
    private void NotesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (NoteModel newNote in e.NewItems!)
                {
                    _board.AddNote(_currentFolder, newNote);
                    newNote.PropertyChanged += NotesOnContentEdited;
                }
                break;
            
            case NotifyCollectionChangedAction.Remove:
                foreach (NoteModel oldNote in e.OldItems!)
                {
                    _board.RemoveNote(_currentFolder, oldNote.Guid);
                    oldNote.PropertyChanged -= NotesOnContentEdited;
                }
                break;
            
            case NotifyCollectionChangedAction.Replace:
            case NotifyCollectionChangedAction.Move:
            case NotifyCollectionChangedAction.Reset:
                throw new NotSupportedException();
        }
    }

    private void NotesOnContentEdited(object sender, PropertyChangedEventArgs e)
    {
        if (sender is not NoteModel note)
            return;
        
        _board.EditNote(_currentFolder, note);
    }
}