using System.Text.Json;

namespace Common
{
    public static class Constants
    {
        private const string AppName = "krvnotes";
        private const string Filename = "board.json";

        public static readonly string FolderPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                AppName);

        public static readonly string FilePath = Path.Combine(FolderPath, Filename);

        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public enum FolderType
        {
            Custom,
            Unsorted,
            Archive,
            RecycleBin
        }

        private static IEnumerable<DefaultFolder> _defaultFolders;
        public static IEnumerable<DefaultFolder> DefaultFolders => _defaultFolders ??= CreateDefaultFolders();

        private static IEnumerable<DefaultFolder> CreateDefaultFolders()
        {
            var list = new []
            {
                new DefaultFolder(FolderType.Unsorted, @"unsorted-guid", "Unsorted"),
                new DefaultFolder(FolderType.Archive, @"archive-guid", "Archive"),
                new DefaultFolder(FolderType.RecycleBin, @"recycle-bin-guid", "Recycle bin")
            };

            return list;
        }
    }

    public struct DefaultFolder
    {
        public DefaultFolder(Constants.FolderType folderType, string guid, string name)
        {
            FolderType = folderType;
            Guid = guid;
            Name = name;
        }

        public Constants.FolderType FolderType { get; }
        public string Guid { get; }
        public string Name { get; }
    }
}