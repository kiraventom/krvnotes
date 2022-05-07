using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.Templates;

public class EditorOverlay : Control
{
    public static readonly DependencyProperty CloseNoteCommandProperty =
        DependencyProperty.Register(
            nameof(CloseNoteCommand), 
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
    
    public ICommand CloseNoteCommand
    {
        get => (ICommand)GetValue(CloseNoteCommandProperty);
        set => SetValue(CloseNoteCommandProperty, value);
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