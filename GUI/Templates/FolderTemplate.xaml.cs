using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Common;

namespace GUI.Templates;

public class Folder : RadioButton
{
    public static readonly DependencyProperty FolderTypeProperty =
        DependencyProperty.Register(
            nameof(FolderType),
            typeof(Constants.FolderType),
            typeof(Folder));

    public static readonly DependencyProperty FolderNameProperty =
        DependencyProperty.Register(
            nameof(FolderName),
            typeof(string),
            typeof(Folder));

    public Constants.FolderType FolderType
    {
        get => (Constants.FolderType)GetValue(FolderTypeProperty);
        set => SetValue(FolderTypeProperty, value);
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