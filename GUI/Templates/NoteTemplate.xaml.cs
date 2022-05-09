using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.Templates;

public class Note : Control
{
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(
            nameof(Header), 
            typeof(string), 
            typeof(Note),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text), 
            typeof(string), 
            typeof(Note),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty OpenNoteCommandProperty =
        DependencyProperty.Register(
            nameof(OpenNoteCommand), 
            typeof(ICommand), 
            typeof(Note));
    
    public static readonly DependencyProperty OpenNoteCommandParameterProperty =
        DependencyProperty.Register(
            nameof(OpenNoteCommandParameter), 
            typeof(object), 
            typeof(Note));
    
    public static readonly DependencyProperty DeleteNoteCommandProperty =
        DependencyProperty.Register(
            nameof(DeleteNoteCommand), 
            typeof(ICommand), 
            typeof(Note));
    
    public static readonly DependencyProperty DeleteNoteCommandParameterProperty =
        DependencyProperty.Register(
            nameof(DeleteNoteCommandParameter), 
            typeof(object), 
            typeof(Note));

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