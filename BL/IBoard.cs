namespace BL;

public interface IBoard
{
    bool AddFolder(string name);
    //bool EditFolder(string oldName, string newName);
    bool RemoveFolder(string name);
    IFoldersCollection Folders { get; }
}

public interface IFoldersCollection : IEnumerable<IFolder>
{
    IFolder this[string key] { get; }
}