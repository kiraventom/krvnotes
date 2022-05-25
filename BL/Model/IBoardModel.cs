namespace BL.Model;

public interface IBoardModel
{
    bool AddFolder(string name);
    bool RemoveFolder(string name);
    IFoldersCollection Folders { get; }
}

public interface IFoldersCollection : IEnumerable<IFolderModel>
{
    IFolderModel this[string key] { get; }
}