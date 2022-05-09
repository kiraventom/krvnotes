using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Common;

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

    public static readonly DependencyProperty IsCheckedProperty =
        DependencyProperty.Register(
            nameof(IsChecked),
            typeof(bool),
            typeof(Folder));

    public static readonly DependencyProperty FolderNameProperty =
        DependencyProperty.Register(
            nameof(FolderName),
            typeof(string),
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

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public string FolderName
    {
        get => (string)GetValue(FolderNameProperty);
        set => SetValue(FolderNameProperty, value);
    }
}

[ValueConversion(typeof(FolderWrapper), typeof(bool))]
public class AreFoldersEqual : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values[0] is FolderWrapper f0 && values[1] is FolderWrapper f1 && f0 == f1;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}