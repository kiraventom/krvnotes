using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GUI.Commands;
using BL;

namespace GUI;

public class BoardViewModel : BasicNotifiable
{
    public BoardViewModel()
    {
        try
        {
            _board = Controller.GetBoard();
        }
        catch (WrongAppEntryPointException)
        {
            EventLog.WriteEntry(
                Assembly.GetExecutingAssembly().FullName, 
                "krvnotes should be run from krvnotes-Starter.exe!",
                EventLogEntryType.Error);
            
            Environment.Exit(1);
        }
        
        var loadedNotes =
            _board.Notes.Select(p => 
                new NoteModel(p.Key)
                {
                    Header = p.Value.Header,
                    Text = p.Value.Text
                });

        Notes = new ObservableCollection<NoteModel>();
        Notes.CollectionChanged += NotesOnCollectionChanged;
        foreach (var loadedNote in loadedNotes) 
            Notes.Add(loadedNote);

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
                    _board.Add(newNote.Guid, newNote.Header, newNote.Text);
                    newNote.PropertyChanged += NotesOnContentEdited;
                }
                break;
            
            case NotifyCollectionChangedAction.Remove:
                foreach (NoteModel oldNote in e.OldItems!)
                {
                    _board.Remove(oldNote.Guid);
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
        
        _board.Edit(note.Guid, note.Header, note.Text);
    }
}