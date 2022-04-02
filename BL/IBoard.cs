namespace BL;

public interface IBoard
{
    bool AddFolder(IFolder folder);
    bool EditFolder(string oldName, string newName);
    bool RemoveFolder(string name);
    IEnumerable<IFolder> Folders { get; }
}