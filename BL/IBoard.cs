namespace BL;

public interface IBoard
{
    bool AddFolder(string name);
    bool RemoveFolder(string name);
    IFoldersCollection Folders { get; }
}

public interface IFoldersCollection : IEnumerable<IFolder>
{
    IFolder this[string key] { get; }
}