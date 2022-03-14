using System.Text.Json;
using BL;

namespace Logic;

internal static class Dumper
{
    static Dumper()
    {
        Directory.CreateDirectory(Constants.FolderPath);
    }
    
    public static void Save(IEnumerable<IFolder> folders)
    {
        string json = JsonSerializer.Serialize(folders, Constants.SerializerOptions);
        File.WriteAllText(Constants.FilePath, json);    
    }

    internal static bool TryLoad(out IEnumerable<Folder> folders)
    {
        if (!File.Exists(Constants.FilePath))
        {
            folders = null;
            return false;
        }
        
        string json = File.ReadAllText(Constants.FilePath);
        
        folders = JsonSerializer.Deserialize<IEnumerable<Folder>>(json, Constants.SerializerOptions);
        ArgumentNullException.ThrowIfNull(folders); // I know it's not argument, idc
        return true;
    }
}