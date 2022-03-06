using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.ControlTemplates;

public class EditorOverlay : Control
{
    public static readonly DependencyProperty SaveNoteCommandProperty =
        DependencyProperty.Register(
            nameof(SaveNoteCommand), 
            typeof(ICommand), 
            typeof(EditorOverlay));
    
    public static readonly DependencyProperty CancelEditingCommandProperty =
        DependencyProperty.Register(
            nameof(CancelEditingCommand), 
            typeof(ICommand), 
            typeof(EditorOverlay));
    
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(
            nameof(Header), 
            typeof(string), 
            typeof(EditorOverlay),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text), 
            typeof(string), 
            typeof(EditorOverlay),
            new PropertyMetadata(string.Empty));
    
    public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register(
            nameof(IsActive), 
            typeof(bool), 
            typeof(EditorOverlay),
            new PropertyMetadata(false));
    
    public ICommand SaveNoteCommand
    {
        get => (ICommand)GetValue(SaveNoteCommandProperty);
        set => SetValue(SaveNoteCommandProperty, value);
    }
    
    public ICommand CancelEditingCommand
    {
        get => (ICommand)GetValue(CancelEditingCommandProperty);
        set => SetValue(CancelEditingCommandProperty, value);
    }
    
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
    
    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }
}