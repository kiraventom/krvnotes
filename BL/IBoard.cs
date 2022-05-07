namespace BL;

public interface IBoard
{
    bool AddFolder(string name);
    //bool EditFolder(string oldName, string newName);
    bool RemoveFolder(string name);
    IEnumerable<IFolder> Folders { get; }
}