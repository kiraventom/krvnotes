using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GUI.ControlTemplates;

public class NoteTemplate : Control
{
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(
            nameof(Header), 
            typeof(string), 
            typeof(NoteTemplate),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text), 
            typeof(string), 
            typeof(NoteTemplate),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty OpenNoteCommandProperty =
        DependencyProperty.Register(
            nameof(OpenNoteCommand), 
            typeof(ICommand), 
            typeof(NoteTemplate));
    
    public static readonly DependencyProperty OpenNoteCommandParameterProperty =
        DependencyProperty.Register(
            nameof(OpenNoteCommandParameter), 
            typeof(object), 
            typeof(NoteTemplate));
    
    public static readonly DependencyProperty DeleteNoteCommandProperty =
        DependencyProperty.Register(
            nameof(DeleteNoteCommand), 
            typeof(ICommand), 
            typeof(NoteTemplate));
    
    public static readonly DependencyProperty DeleteNoteCommandParameterProperty =
        DependencyProperty.Register(
            nameof(DeleteNoteCommandParameter), 
            typeof(object), 
            typeof(NoteTemplate));

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public ICommand OpenNoteCommand
    {
        get => (ICommand)GetValue(OpenNoteCommandProperty);
        set => SetValue(OpenNoteCommandProperty, value);
    }
    
    public object OpenNoteCommandParameter
    {
        get => GetValue(OpenNoteCommandParameterProperty);
        set => SetValue(OpenNoteCommandParameterProperty, value);
    }

    public ICommand DeleteNoteCommand
    {
        get => (ICommand)GetValue(DeleteNoteCommandProperty);
        set => SetValue(DeleteNoteCommandProperty, value);
    }
    
    public object DeleteNoteCommandParameter
    {
        get => GetValue(DeleteNoteCommandParameterProperty);
        set => SetValue(DeleteNoteCommandParameterProperty, value);
    }
}