using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;
using Common;

namespace GUI.Icons.FolderIcons
{
    internal static class IconManager
    {
        public static Path GetFolderIcon(Constants.FolderType type)
        {
            var path = type switch
            {
                Constants.FolderType.Custom => Application.Current.FindResource("NoteIcon"),
                Constants.FolderType.Unsorted => Application.Current.FindResource("NoteIcon"),
                Constants.FolderType.Archive => Application.Current.FindResource("ArchiveIcon"),
                Constants.FolderType.RecycleBin => Application.Current.FindResource("RecycleBinIcon"),
                _ => throw new NotImplementedException()
            };

            return path as Path;
        }
    }

    [ValueConversion(typeof(Constants.FolderType), typeof(Path))]
    public class FolderTypeToIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Constants.FolderType folderType && Enum.IsDefined(folderType))
                return IconManager.GetFolderIcon(folderType);

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
