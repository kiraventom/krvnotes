using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GUI.Commands;
using BL;

namespace GUI;

public class BoardViewModel : BasicNotifier
{
    public BoardViewModel()
    {
        try
        {
            Board = Controller.GetBoard();
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
            Board.Notes
                .Select(p => new NoteModel(p.Key)
                {
                    Header = p.Value.Header,
                    Text = p.Value.Text
                });

        Notes = new ObservableCollection<NoteModel>(loadedNotes);

        CreateNoteCommand = new Command(() => CurrentNote = new NoteModel());
        OpenNoteCommand = new Command<NoteModel>(note => CurrentNote = note, note => note is not null);
        SaveNoteCommand = new Command(SaveAction);
        DeleteNoteCommand = new Command<NoteModel>(DeleteAction,  note => note is not null);
    }


    // Exposed properties
    public ObservableCollection<NoteModel> Notes { get; set; }

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

    private NoteModel _currentNote;

    public bool IsNoteEditActive => CurrentNote is not null;


    // Commands
    public ICommand CreateNoteCommand { get; }
    public ICommand OpenNoteCommand { get; }
    public ICommand SaveNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }


    // Commands actions
    private void SaveAction()
    {
        if (!string.IsNullOrWhiteSpace(CurrentNote.Header) ||
            !string.IsNullOrWhiteSpace(CurrentNote.Text))
        {
            if (!Notes.Contains(CurrentNote))
                Notes.Add(CurrentNote);
            
            Board.AddOrReplace(CurrentNote.Guid, CurrentNote.Header, CurrentNote.Text);
        }

        CurrentNote = null;
    }
    
    private void DeleteAction(NoteModel note)
    {
        Notes.Remove(note);
        Board.Remove(note!.Guid);
    }
    
    // Logic
    private IBoard Board { get; }
}