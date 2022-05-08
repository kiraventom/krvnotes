using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Common;
using GUI.Icons.FolderIcons;

namespace GUI.Templates;

public class Folder : ContentControl
{
    public static readonly DependencyProperty PickFolderCommandProperty =
        DependencyProperty.Register(
            nameof(PickFolderCommand), 
            typeof(ICommand), 
            typeof(Folder));
    
    public static readonly DependencyProperty PickFolderCommandParameterProperty =
        DependencyProperty.Register(
            nameof(PickFolderCommandParameter), 
            typeof(object), 
            typeof(Folder));

    public static readonly DependencyProperty GroupNameProperty =
        DependencyProperty.Register(
            nameof(GroupName),
            typeof(string),
            typeof(Folder));

    public static readonly DependencyProperty FolderTypeProperty =
        DependencyProperty.Register(
            nameof(FolderType),
            typeof(Constants.FolderType),
            typeof(Folder));

    public ICommand PickFolderCommand
    {
        get => (ICommand)GetValue(PickFolderCommandProperty);
        set => SetValue(PickFolderCommandProperty, value);
    }
    
    public object PickFolderCommandParameter
    {
        get => GetValue(PickFolderCommandParameterProperty);
        set => SetValue(PickFolderCommandParameterProperty, value);
    }

    public string GroupName
    {
        get => (string)GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    public Constants.FolderType FolderType
    {
        get => (Constants.FolderType)GetValue(FolderTypeProperty);
        set => SetValue(FolderTypeProperty, value);
    }
}