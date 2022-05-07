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

        public static readonly Random Random = new();

        public enum FolderType
        {
            Custom,
            Unsorted,
            Archive,
            RecycleBin
        }

        public static IEnumerable<string> DefaultFolders => Enum.GetNames<FolderType>().Skip(1);
    }
}
